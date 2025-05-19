using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public sealed record BookSentence
    {
        [JsonInclude]
        public string? OriginSentence { get; init; }
        
        [JsonInclude]
        public string? JudgeSentence { get; init; }
    }
}
