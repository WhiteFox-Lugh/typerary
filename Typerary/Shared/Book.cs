namespace Typerary.Shared
{
    public class Book
    {
        public string? Title { get; set; }
        public BookContent[]? Content { get; set; }
    }

    public class BookContent
    {
        public string? Section { get; set; }
        public BookSentence[]? Sentences { get; set; }
    }

    public class BookSentence
    {
        public string? OriginSentence { get; set; }
        public string? JudgeSentence { get; set; }
    }
}
