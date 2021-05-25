using System;
using CommandLine;

namespace Redminer
{
    public class Arguments
    {
        [Option('i', "issue", Required = true, HelpText = "Set issue id.")]
        public int IssueId { get; set; }

        [Option('f', "from", Required = false, HelpText = "Set spending date from.")]
        public DateTime From { get; set; } = DateTime.Today;

        [Option('t', "to", Required = false, HelpText = "Set spending date to.")]
        public DateTime? To { get; set; }
        
        [Option('d', "dates", Required = false, HelpText = "Set spending dates.")]
        public string Dates { get; set; }

        [Option('h', "hour", Required = true, HelpText = "Set spended hours.")]
        public double Hours { get; set; }

        [Option('a', "activity", Required = false, HelpText = "Set activity. Development, Review or Analysis.")]
        public ActivityType ActivityId { get; set; } = ActivityType.Review;

        [Option('c', "comment", Required = false, HelpText = "Set comment.")]
        public string Comments { get; set; } = ".";
        
        [Option('k', "apiKey", Required = true, HelpText = "Set Redmine API key.")]
        public string ApiKey { get; set; }
    }
}