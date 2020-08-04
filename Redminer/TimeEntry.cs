using System;
using System.Text.Json.Serialization;
using CommandLine;

namespace Redminer
{
    public class TimeEntry
    {
        [JsonPropertyName("issue_id")]
        public int IssueId { get; set; }

        [JsonPropertyName("spent_on")]
        public DateTime SpendOn { get; set; } = DateTime.Today;

        [JsonPropertyName("hours")]
        public double Hours { get; set; }

        [JsonPropertyName("activity_id")]
        public ActivityType ActivityId { get; set; } = ActivityType.Review;

        [JsonPropertyName("comments")]
        public string Comments { get; set; } = ".";
    }
}