using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.BotBuilderSamples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;

namespace QnABot.Pages
{
    public class IndexModel : PageModel
    {
        const string TokenGenerationUrl = "https://directline.botframework.com/v3/directline/tokens/generate";

        public IndexModel(IConfiguration configuration)
        {
            BotSecret = configuration["BotSecret"];
            HideUploadButton = bool.Parse(configuration["HideUploadButton"]);
            BotAvatarInitials = configuration["BotAvatarInitials"];
            UserAvatarInitials = configuration["UserAvatarInitials"];
            Language = configuration["Language"];

            DLToken = GetTokenAsync().ConfigureAwait(false).GetAwaiter().GetResult().token;
        }
        public string DLToken { get; set; }

        // Dynamic values (not appsettings values)
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = "covid19user";
        public string ConversationId { get; set; } = Guid.NewGuid().ToString();

        //Index.cshtml settings
        public string BotSecret { get; set; }
        public bool HideUploadButton { get; set; }
        public string BotAvatarInitials { get; set; }
        public string UserAvatarInitials { get; set; }
        public string Language { get; set; }

        public void OnGet()
        {

        }

        public async Task<DirectLineToken> GetTokenAsync()
        {

            using (var request = new HttpRequestMessage(HttpMethod.Post, TokenGenerationUrl))
            {
                // For more information on exchanging a secret for a token see:
                //  https://docs.microsoft.com/en-us/azure/bot-service/rest-api/bot-framework-rest-direct-line-3-0-authentication#generate-token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", BotSecret);
                request.Content = new StringContent(JsonConvert.SerializeObject(new { User = new { Id = UserId } }), Encoding.UTF8, "application/json");

                using (var response = await Startup.HttpClient.SendAsync(request).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        var dlToken = JsonConvert.DeserializeObject<DirectLineToken>(body);
                        dlToken.userId = UserId;
                        return dlToken;
                    }
                }
            }

            return null;
        }

        public class DirectLineToken
        {
            public string userId { get; set; }
            public string conversationId { get; set; }
            public string token { get; set; }
            public int expires_in { get; set; }
            public string streamUrl { get; set; }
        }

    }
}