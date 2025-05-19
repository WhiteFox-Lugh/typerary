using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public record BookSentence
    {
        [JsonInclude]
        public string? OriginSentence { get; set; }
        
        [JsonInclude]
        public string? JudgeSentence { get; set; }
    }
}
