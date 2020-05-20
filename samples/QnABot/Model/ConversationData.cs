using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QnABot.Model
{
    public class ConversationData
    {
        public string FeedbackType { get; set; }
        public string PreviousQuestion { get; set; }
        public string PreviousAnswer { get; set; }
        public string LanguagePreference { get; set; } = "en";
        public bool LanguageChangeRequested { get; set; }
        public bool LanguageChangeDetected { get; set; }
    }
}
