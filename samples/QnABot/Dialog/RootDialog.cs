// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.QnA.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace Microsoft.BotBuilderSamples.Dialog
{
    /// <summary>
    /// This is an example root dialog. Replace this with your applications.
    /// </summary>
    public class RootDialog : ComponentDialog
    {
        /// <summary>
        /// QnA Maker initial dialog
        /// </summary>
        private const string InitialDialog = "initial-dialog";
        IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootDialog"/> class.
        /// </summary>
        /// <param name="services">Bot Services.</param>
        public RootDialog(IBotServices services, IConfiguration configuration)
            : base("root")
        {
            _configuration = configuration;

            AddDialog(new QnAMakerBaseDialog(services, configuration));

            AddDialog(new WaterfallDialog(InitialDialog)
               .AddStep(InitialStepAsync));

            // The initial child Dialog to run.
            InitialDialogId = InitialDialog;
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set values for generate answer options.
            var qnamakerOptions = new QnAMakerOptions
            {
                ScoreThreshold = float.Parse(_configuration["DefaultThreshold"]),
                Top = int.Parse(_configuration["NumberOfAnswersToReturn"]),
                Context = new QnARequestContext()
            };

            var noAnswer = (Activity)Activity.CreateMessageActivity();
            noAnswer.Text = _configuration["DefaultNoAnswer"];

            var defaultCardNoMatchResponse = (Activity)Activity.CreateMessageActivity();
            defaultCardNoMatchResponse.Text = _configuration["DefaultCardNoMatchResponse"];

            // Set values for dialog responses.
            var qnaDialogResponseOptions = new QnADialogResponseOptions
            {
                NoAnswer = noAnswer,
                CardNoMatchResponse = defaultCardNoMatchResponse,
                CardNoMatchText = _configuration["DefaultCardNoMatchText"],
                ActiveLearningCardTitle = _configuration["DefaultCardTitle"]
            };

            var dialogOptions = new Dictionary<string, object>
            {
                { "QnAOptions", qnamakerOptions },
                { "QnADialogResponseOptions", qnaDialogResponseOptions }
            };

           return await stepContext.BeginDialogAsync(nameof(QnAMakerDialog), dialogOptions, cancellationToken);
        }
    }
}
