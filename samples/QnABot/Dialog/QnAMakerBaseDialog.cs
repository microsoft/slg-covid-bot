// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.QnA.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace Microsoft.BotBuilderSamples.Dialog
{
    /// <summary>
    /// QnAMaker action builder class
    /// </summary>
    public class QnAMakerBaseDialog : QnAMakerDialog
    {
        IConfiguration _configuration;

        private readonly IBotServices _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="QnAMakerBaseDialog"/> class.
        /// Dialog helper to generate dialogs.
        /// </summary>
        /// <param name="services">Bot Services.</param>
        public QnAMakerBaseDialog(IBotServices services, IConfiguration configuration): base()
        {
            this._services = services;
            _configuration = configuration;
        }

#pragma warning disable CS1998  
        protected async override Task<IQnAMakerClient> GetQnAMakerClientAsync(DialogContext dc)
        {
            return this._services?.QnAMakerService;
        }
#pragma warning restore CS1998

        protected override Task<QnAMakerOptions> GetQnAMakerOptionsAsync(DialogContext dc)
        {
            return Task.FromResult(new QnAMakerOptions
            {
                ScoreThreshold = float.Parse(_configuration["DefaultThreshold"]),
                Top = int.Parse(_configuration["NumberOfAnswersToReturn"]),
                QnAId = 0,
                RankerType = "Default",
                IsTest = false
            });
        }

#pragma warning disable CS1998
        protected async override Task<QnADialogResponseOptions> GetQnAResponseOptionsAsync(DialogContext dc)
        {
            var noAnswer = (Activity)Activity.CreateMessageActivity();
            noAnswer.Text = _configuration["DefaultNoAnswer"];

            var cardNoMatchResponse = (Activity)MessageFactory.Text(_configuration["DefaultCardNoMatchResponse"]);

            var responseOptions = new QnADialogResponseOptions
            {
                ActiveLearningCardTitle = _configuration["DefaultCardTitle"],
                CardNoMatchText = _configuration["DefaultCardNoMatchText"],
                NoAnswer = noAnswer,
                CardNoMatchResponse = cardNoMatchResponse,
            };

            return responseOptions;
        }
#pragma warning restore CS1998

        async public override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            return await base.BeginDialogAsync(dc, options, cancellationToken);
        }
    }
}
