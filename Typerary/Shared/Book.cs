using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public sealed record Book
    {
        [JsonInclude]
        public string? Title { get; init; }
        
        [JsonInclude]
        public BookContent[]? Content { get; init; }
    }
}
