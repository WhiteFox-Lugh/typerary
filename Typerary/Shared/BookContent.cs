using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public class BookContent
    {
        [JsonInclude]
        public string? Section { get; set; }
        
        [JsonInclude]
        public BookSentence[] Sentences { get; set; }
    }
}
