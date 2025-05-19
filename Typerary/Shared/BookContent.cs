using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public sealed record BookContent
    {
        [JsonInclude]
        public string? Section { get; init; }
        
        [JsonInclude]
        public BookSentence[]? Sentences { get; init; }
    }
}
