namespace Typerary.Shared
{
    public class WorkMetadata
    {
        public List<WorkData> WorkList { get; set; }
    }

    public class WorkData
    {
        public string WorkId { get; set; }

        public string Title { get; set; }

        public string TitleYomi { get; set; }

        public string? SubTitle { get; set; }

        public string Author { get; set; }

        public string AuthorYomi { get; set; }

        public Uri WorkUri { get; set; }
    }
}
