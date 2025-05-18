using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public class Book
    {
        [JsonInclude]
        public string? Title { get; set; }
        
        [JsonInclude]
        public BookContent[]? Content { get; set; }
    }
}
