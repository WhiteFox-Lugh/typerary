namespace Typerary.Shared
{
    public class Book
    {
        public string Title { get; set; }
        public BookData[] Content { get; set; }
    }

    public class BookData
    {
        public string Section { get; set; }
        public Sentence[] Sentences { get; set; }
    }

    public class Sentence
    {
        public string OriginSentence { get; set; }
        public string JudgeSentence { get; set; }
    }
}
