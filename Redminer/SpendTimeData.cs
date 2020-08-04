using System.Text.Json.Serialization;

namespace Redminer
{
    public class SpendTimeData
    {
        [JsonPropertyName("time_entry")] public TimeEntry TimeEntry { get; set; }
    }
}