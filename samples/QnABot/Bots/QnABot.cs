// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QnABot.Model;
using static Microsoft.Bot.Builder.Dialogs.Choices.Channel;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class QnABot<T> : ActivityHandler where T : Microsoft.Bot.Builder.Dialogs.Dialog
    {
        protected readonly BotState _conversationState;
        protected readonly Microsoft.Bot.Builder.Dialogs.Dialog Dialog;
        protected readonly BotState _userState;
        IConfiguration _configuration;
        static public string[] SupportedLanguages = new string[] { "en", "es", "fr", "ja", "zh", "de" };

        public QnABot(ConversationState conversationState, UserState userState, T dialog, IConfiguration configuration)
        {
            _conversationState = conversationState;
            this._userState = userState;
            Dialog = dialog;
            _configuration = configuration;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Grab the conversation data
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());
            string utterance = null;

            // If we need to switch languages
            if (conversationData.LanguageChangeRequested || conversationData.LanguageChangeDetected)
            {
                string language = utterance;
                var fullWelcomePrompt = _configuration["WelcomeCardTitle"] + ". " + _configuration["WelcomePrompt"];
                string detection_re_welcomeMessage = $"{_configuration["LanguageTransitionPrompt"]}\r\n\r\n{fullWelcomePrompt}\r\n\r\n{_configuration["QuestionSegue"]}";
                var dectection_re_welcomePrompt = MessageFactory.Text(detection_re_welcomeMessage);
                var languageChange_re_welcomePrompt = MessageFactory.Text(fullWelcomePrompt);

                if (conversationData.LanguageChangeDetected)
                {
                    // Reset this flag
                    conversationData.LanguageChangeDetected = false;

                    // Re-welcome user in their language
                    await turnContext.SendActivityAsync(dectection_re_welcomePrompt, cancellationToken);

                    // Now run the Dialog with the new message Activity so answer will follow the re-welcome prompt.
                    await Dialog.RunAsync(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
                }
                else // then user explicitly requested a language change
                {
                    // Reset this flag
                    conversationData.LanguageChangeRequested = false;

                    // Re-welcome user in their language
                    await turnContext.SendActivityAsync(languageChange_re_welcomePrompt, cancellationToken);
                }
            }
            else 
            {
                // Run the Dialog with the new message Activity.
                await Dialog.RunAsync(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
            }
        }

        private string ConvertToUtterance(ITurnContext<IMessageActivity> turnContext)
        {
            string utterance = null;

            // If this is a postback, check to see if its a "preferred language" choice
            if (turnContext.Activity.Value != null)
            {
                // Split out the language choice
                string[] tokens = turnContext.Activity.Value
                                                        .ToString()
                                                        .Replace('{', ' ')
                                                        .Replace('}', ' ')
                                                        .Replace('"', ' ')
                                                        .Trim()
                                                        .Split(':');

                // If postback is a language choice then grab that choice
                if (tokens.Count() == 2 && tokens[0].Trim() == "LanguagePreference")
                    turnContext.Activity.Text = utterance = tokens[1].Trim();
            }
            else
            {
                utterance = turnContext.Activity.Text.ToLower();
            }

            return utterance;
        }

        private bool IsLanguageChangeRequested(string utterance)
        {
            if (string.IsNullOrEmpty(utterance))
            {
                return false;
            }

            // If the user requested a language change through the suggested actions with values "es" or "en",
            // simply change the user's language preference in the conversation state.
            // The translation middleware will catch this setting and translate both ways to the user's
            // selected language.
            return SupportedLanguages.Contains(utterance);
        }

        async private Task<string> GetDetectedLanguageAsync(string utterance, string currentLanguage)
        {
            string isLanguageChangeDetected = null;
            
            if (string.IsNullOrEmpty(utterance))
            {
                return null;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Post, _configuration["TextAnalyticsEndpoint"]))
            {
                string body = $"{{ \"documents\": [ {{ \"id\": \"1\", \"text\": \"{utterance}\" }} ] }}";

                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                Startup.HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration["TextAnalyticsKey"]);
                Startup.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await Startup.HttpClient.SendAsync(request).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        var detectionResult = JsonConvert.DeserializeObject<LanguageDetectionResponse>(content);

                        if (detectionResult.documents.Count() > 0 && 
                            detectionResult.documents[0].detectedLanguages.Count() > 0 &&
                            detectionResult.documents[0].detectedLanguages[0].score > 0.5)
                        {
                            // If current language is different than the detected language, then grab new language
                            if (currentLanguage != detectionResult.documents[0].detectedLanguages[0].iso6391Name)
                                isLanguageChangeDetected = detectionResult.documents[0].detectedLanguages[0].iso6391Name;
                        }
                    }
                    else
                    {
                        // ToDo: Log error
                    }
                }
            }

            return isLanguageChangeDetected;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            bool useBasicWelcomeCard = false;


            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    // If an adaptive card for the welcome prompts exists, use it
                    if (bool.TryParse(_configuration["UseBasicWelcomeCard"], out useBasicWelcomeCard) && useBasicWelcomeCard)
                    {
                        var welcomeCard = CreateAdaptiveCardAttachment();
                        var response = MessageFactory.Attachment(welcomeCard);

                        await turnContext.SendActivityAsync(response, cancellationToken);
                    }
                    else
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text(_configuration["WelcomePrompt"]), cancellationToken);
                    }
                }
            }
        }

        //protected override async Task OnEventAsync(ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        //{
        //    if (turnContext.Activity.Name == "webchat/join")
        //    {
        //        await turnContext.SendActivityAsync(MessageFactory.Text(_configuration["WelcomePrompt"]), cancellationToken);
        //    }
        //    else if (turnContext.Activity.Name == "webchat/feedback")
        //    {
        //        var originalQuestion = HttpUtility.UrlDecode(turnContext.Activity.Value as string, Encoding.UTF8);
        //        await turnContext.SendActivityAsync(MessageFactory.Text("Thank you for this feedback. It will help us improve."), cancellationToken);
        //    }

        //    await base.OnEventAsync(turnContext, cancellationToken);
        //}

        public static Attachment CreateWelcomePrompt(string answer)
        {
            // combine path for cross platform support
            string[] paths = { ".", "Resources", "WelcomePrompt.json" };
            string adaptiveCardTemplate = File.ReadAllText(Path.Combine(paths));
            string adaptiveCardJson = string.Format(adaptiveCardTemplate, answer);

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }

        public static Attachment CreateAnswerWrapper(string answer)
        {
            // combine path for cross platform support
            string[] paths = { ".", "Resources", "AnswerWrapper.json" };
            string adaptiveCardTemplate = File.ReadAllText(Path.Combine(paths));
            string adaptiveCardJson = string.Format(adaptiveCardTemplate, answer);

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }

        // Load attachment from embedded resource.
        private Attachment CreateAdaptiveCardAttachment()
        {
            // combine path for cross platform support
            string[] paths = { ".", "Resources", "WelcomeCard.json" };
            string adaptiveCardTemplate = File.ReadAllText(Path.Combine(paths));

            // Replace any placeholders in the WelcomeAdaptiveCard from appsetting.json
            string welcomeAdaptiveCard = adaptiveCardTemplate
                .Replace("{WelcomeCardImageBase64}", _configuration["WelcomeCardImageBase64"], true, CultureInfo.CurrentCulture)
                .Replace("{WelcomeCardTitle}", _configuration["WelcomeCardTitle"], true, CultureInfo.CurrentCulture)
                .Replace("{WelcomePrompt}", _configuration["WelcomePrompt"], true, CultureInfo.CurrentCulture)
                .Replace("{WelcomeCardContentAreaBackgroundImage}", _configuration["WelcomeCardContentAreaBackgroundImage"], true, CultureInfo.CurrentCulture)
                .Replace("{WelcomeCardFlagAreaBackgroundImage}", _configuration["WelcomeCardFlagAreaBackgroundImage"], true, CultureInfo.CurrentCulture);

            return new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(welcomeAdaptiveCard),
            };
        }
    }
}
