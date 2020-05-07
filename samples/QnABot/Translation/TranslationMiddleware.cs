// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Dialog;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QnABot.Model;

namespace Microsoft.BotBuilderSamples.Translation
{
    /// <summary>
    /// Middleware for translating text between the user and bot.
    /// Uses the Microsoft Translator Text API.
    /// </summary>
    public class TranslationMiddleware : IMiddleware
    {
        private readonly MicrosoftTranslator _translator;
        ConversationState _conversationState;
        IConfiguration _configuration;
        static public string[] SupportedLanguages = new string[] { "en", "es", "fr", "ja", "zh", "de" };

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationMiddleware"/> class.
        /// </summary>
        /// <param name="translator">Translator implementation to be used for text translation.</param>
        /// <param name="languageStateProperty">State property for current language.</param>
        public TranslationMiddleware(MicrosoftTranslator translator, ConversationState conversationState, IConfiguration configuration)
        {
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));
            _conversationState = conversationState;
            _configuration = configuration;
        }

        /// <summary>
        /// Processes an incoming activity.
        /// </summary>
        /// <param name="turnContext">Context object containing information for a single turn of conversation with a user.</param>
        /// <param name="next">The delegate to call to continue the bot middleware pipeline.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Grab the conversation data
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());
            string utterance = null;
            string detectedLanguage = null;


            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                utterance = ConvertToUtterance(turnContext);

                if (IsLanguageChangeRequested(utterance))
                {
                    conversationData.LanguageChangeRequested = true;
                    conversationData.LanguagePreference = utterance;
                }
                else
                {
                    // Detect language unless its been optimized out after initial language choice
                    if (_configuration["SkipLanguageDetectionAfterInitialChoice"].ToLower() == "false" ||
                    conversationData.LanguagePreference.ToLower() == _configuration["TranslateTo"].ToLower())
                    {
                        detectedLanguage = await GetDetectedLanguageAsync(utterance, conversationData.LanguagePreference);
                        if (detectedLanguage != null)
                        {
                            if (detectedLanguage != conversationData.LanguagePreference)
                            {
                                conversationData.LanguageChangeDetected = true;
                                conversationData.LanguagePreference = detectedLanguage;
                            }
                        }
                    }
                }

                var translate = ShouldTranslateAsync(turnContext, conversationData.LanguagePreference, cancellationToken);

                if (!conversationData.LanguageChangeRequested && translate)
                {
                    if (turnContext.Activity.Type == ActivityTypes.Message)
                    {
                        turnContext.Activity.Text = await _translator.TranslateAsync(turnContext.Activity.Text, _configuration["TranslateTo"], cancellationToken);
                    }
                }

                turnContext.OnSendActivities(async (newContext, activities, nextSend) =>
                {
                // Grab the conversation data
                var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
                    var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

                    string userLanguage = conversationData.LanguagePreference;
                    bool shouldTranslate = userLanguage != _configuration["TranslateTo"];

                // Translate messages sent to the user to user language
                if (shouldTranslate)
                    {
                        List<Task> tasks = new List<Task>();
                        foreach (Activity currentActivity in activities.Where(a => a.Type == ActivityTypes.Message))
                        {
                            tasks.Add(TranslateMessageActivityAsync(currentActivity.AsMessageActivity(), userLanguage));
                        }

                        if (tasks.Any())
                        {
                            await Task.WhenAll(tasks).ConfigureAwait(false);
                        }
                    }

                    return await nextSend();
                });

                turnContext.OnUpdateActivity(async (newContext, activity, nextUpdate) =>
                {
                // Grab the conversation data
                var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
                    var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

                    string userLanguage = conversationData.LanguagePreference;
                    bool shouldTranslate = userLanguage != _configuration["TranslateTo"];

                // Translate messages sent to the user to user language
                if (activity.Type == ActivityTypes.Message)
                    {
                        if (shouldTranslate)
                        {
                            await TranslateMessageActivityAsync(activity.AsMessageActivity(), userLanguage);
                        }
                    }

                    return await nextUpdate();
                });
            }

            await next(cancellationToken).ConfigureAwait(false);
        }

        private async Task TranslateMessageActivityAsync(IMessageActivity activity, string targetLocale, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (activity.Type == ActivityTypes.Message)
            {
                activity.Text = await _translator.TranslateAsync(activity.Text, targetLocale);
            }
        }

        private bool ShouldTranslateAsync(ITurnContext turnContext, string usersLanguage, CancellationToken cancellationToken = default(CancellationToken))
        {
            return turnContext.Activity.Text != null && !Microsoft.BotBuilderSamples.Bots.QnABot<RootDialog>.SupportedLanguages.Contains(turnContext.Activity.Text.ToLower()) && usersLanguage != _configuration["TranslateTo"];
        }

        async private Task<string> GetDetectedLanguageAsync(string utterance, string currentLanguage)
        {
            string detectedLanguage = null;

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
                            detectedLanguage = detectionResult.documents[0].detectedLanguages[0].iso6391Name.Substring(0,2);
                        }
                    }
                    else
                    {
                        // ToDo: Log error
                    }
                }
            }

            return detectedLanguage;
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

        private string ConvertToUtterance(ITurnContext turnContext)
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
    }
}
