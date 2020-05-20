using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QnABot.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace QnABot.Utility
{
    public class CustomQnaMakerClient : QnAMaker, IQnAMakerClient
    {
        ConversationState _conversationState;
        IConfiguration _configuration;

        public CustomQnaMakerClient(QnAMakerEndpoint endpoint, IConfiguration configuration, ConversationState conversationState)
            : base(endpoint)
        {
            _configuration = configuration;
            _conversationState = conversationState;
        }

        async protected override Task OnQnaResultsAsync(QueryResult[] queryResults, ITurnContext turnContext, Dictionary<string, string> telemetryProperties = null, Dictionary<string, double> telemetryMetrics = null, CancellationToken cancellationToken = default)
        {
            var highestScoredAnswer = queryResults.OrderByDescending(i => i.Score).FirstOrDefault();

            // Append feeback prompt if one was provided
            if (highestScoredAnswer != null && !string.IsNullOrWhiteSpace(_configuration["FeedbackPrompt"]))
            {
                bool isFeedbackKeyword = false;
                string[] feedbackKeywords = _configuration["FeedbackKeywords"].Split(",");
                var encodedQuestion = HttpUtility.UrlEncode(turnContext.Activity.Text);

                // Grab the conversation data
                var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
                var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

                foreach (string feedbackKeyword in feedbackKeywords)
                {
                    if (string.Compare(encodedQuestion.Trim(), HttpUtility.UrlEncode(feedbackKeyword.Trim())) == 0)
                    {
                        conversationData.FeedbackType = feedbackKeyword.Trim();
                        isFeedbackKeyword = true;
                        break;
                    }
                }

                // If the utterance is not one of the feeback keywords then append the feedback prompt
                if (!isFeedbackKeyword)
                {
                    conversationData.PreviousQuestion = turnContext.Activity.Text;
                    conversationData.PreviousAnswer = highestScoredAnswer.Answer;

                    highestScoredAnswer.Answer += _configuration["FeedbackPrompt"];
                }
                else 
                {
                    // If there is a webhook then use it
                    if (_configuration["FeedbackWebhook"] != null)
                    {
                        using (var request = new HttpRequestMessage(HttpMethod.Post, _configuration["FeedbackWebhook"]))
                        {
                            string body = string.Format("{{ \"feedbackType\": \"{0}\", \"question\": \"{1}\", \"wrongAnswer\": \"{2}\" }}", char.ToUpper(conversationData.FeedbackType[0]) + conversationData.FeedbackType.Substring(1), conversationData.PreviousQuestion, conversationData.PreviousAnswer);

                            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                            using (var response = await Startup.HttpClient.SendAsync(request).ConfigureAwait(false))
                            {
                                if (!response.IsSuccessStatusCode)
                                {
                                    // ToDo: Log error
                                }
                            }
                        }
                    }
                }
            }

            await base.OnQnaResultsAsync(queryResults, turnContext, telemetryProperties, telemetryMetrics, cancellationToken);
        }
    }
}
