using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker.Models;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CovidDataImportFunction
{
    public static class CovidDataImport
    {
        private static List<CovidData> data = new List<CovidData>();

        [FunctionName("CovidDataImport")]
        public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"CovidDateImport function executed at: {DateTime.Now}");
            try
            {
                //Config
                string kbId = System.Environment.GetEnvironmentVariable("kbId");
                string apiKey = System.Environment.GetEnvironmentVariable("apiKey");
                string botEndpoint = System.Environment.GetEnvironmentVariable("botEndpoint");
                int questionId = int.Parse(System.Environment.GetEnvironmentVariable("questionId"));
                string state = System.Environment.GetEnvironmentVariable("state");
                log.LogInformation($"kbId: {kbId}, apiKey: {apiKey}, botEndpoint: {botEndpoint}, questionId {questionId.ToString()}, state: {state}");


                //Get Date to load data
                string month = DateTime.Now.ToString("MM");
                string day = (DateTime.Now.Day - 1).ToString("d2");
                string year = DateTime.Now.ToString("yyyy");
                string filename = string.Format($"{month}-{day}-{year}.csv");
                string asOf = string.Format($"{month}-{day}-{year}");

                //Load COVID Data from GitHub
                data.Clear();
                HttpClient httpClient = new HttpClient();
                string rawData = httpClient.GetStringAsync("https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + filename).Result;
                StringReader stringReader = new StringReader(rawData);
                string dataLine = string.Empty;

                //skip header
                dataLine = stringReader.ReadLine();
                string separatorChar = ",";
                while (true)
                {
                    dataLine = stringReader.ReadLine();
                    if (dataLine != null)
                    {
                        Regex regx = new Regex(separatorChar + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        var dataCols = regx.Split(dataLine);
                        data.Add(new CovidData() { FIPS = (dataCols[0].Trim() != string.Empty) ? int.Parse(dataCols[0]) : (int?)null, Admin2 = dataCols[1], Province_State = dataCols[2], Country_Region = dataCols[3], Last_Update = DateTime.Parse(dataCols[4]), Lat = Double.Parse(dataCols[5]), Long = Double.Parse(dataCols[6]), Confirmed = Int32.Parse(dataCols[7]), Deaths = Int32.Parse(dataCols[8]), Recovered = Int32.Parse(dataCols[9]), Active = Int32.Parse(dataCols[10]), Combined_key = dataCols[11] });
                    }
                    else
                    {
                        break;
                    }
                }

                //Find data for specific state
                var dataItem = data.Where(x => x.Province_State.ToLower() == state.ToLower());
                var confirmed = dataItem.Sum(x => x.Confirmed);
                var deaths = dataItem.Sum(x => x.Deaths);
                var recovered = dataItem.Sum(x => x.Recovered);
                var active = dataItem.Sum(x => x.Active);

                var client = new QnAMakerClient(new ApiKeyServiceClientCredentials(apiKey)) { Endpoint = botEndpoint };

                var x = UpdateKB(client, kbId, questionId, state, confirmed, deaths, recovered, active, asOf, log);
                x.Wait();

                //Publish update
                var p = client.Knowledgebase.PublishAsync(kbId);
                p.Wait();

                log.LogInformation(x.Status.ToString());
            }
            catch (Exception ex)
            {
                log.LogInformation("Error: " + ex.ToString());
                
            }
        }

        private static async Task UpdateKB(IQnAMakerClient client, string kbId, int questionId, string state, int confirmed, int deaths, int recovered, int active, string asOf, ILogger log)
        {
            // Update kb

            UpdateKbOperationDTOUpdate toUpdate = new UpdateKbOperationDTOUpdate();
            toUpdate.QnaList = new List<UpdateQnaDTO> { new UpdateQnaDTO { Id = questionId, Answer = string.Format($"As of {asOf} {state} has {confirmed} confirmed, {recovered} recovered, {active} active, and {deaths} deaths from Covid-19. Data is loaded daily from the Johns Hopkins CSSE data set.") } };

            var updateOp = await client.Knowledgebase.UpdateAsync(kbId, new UpdateKbOperationDTO
            {
                // Create JSON of changes 
                Add = null,
                Update = toUpdate,
                Delete = null
            });

            // Loop while operation is success
            updateOp = await MonitorOperation(client, updateOp, log);
        }

        private static async Task<Operation> MonitorOperation(IQnAMakerClient client, Operation operation, ILogger log)
        {
            // Loop while operation is success
            for (int i = 0;
                i < 20 && (operation.OperationState == OperationStateType.NotStarted || operation.OperationState == OperationStateType.Running);
                i++)
            {
                log.LogInformation("Waiting for operation: {0} to complete.", operation.OperationId);
                await Task.Delay(5000);
                operation = await client.Operations.GetDetailsAsync(operation.OperationId);
            }

            if (operation.OperationState != OperationStateType.Succeeded)
            {
                throw new Exception($"Operation {operation.OperationId} failed to completed.");
            }
            return operation;
        }
    }
}
