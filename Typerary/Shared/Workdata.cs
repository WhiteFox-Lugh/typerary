namespace Typerary.Shared
{
    public class WorkMetadata
    {
        public List<WorkData>? WorkList { get; set; }
    }

    public class WorkData
    {
        private const string DefaultScreenName = "---";
        private const Radzen.BadgeStyle DefaultBadgeStyle = Radzen.BadgeStyle.Dark;
        private static readonly Dictionary<string, (string screenName, Radzen.BadgeStyle badgeStyle)> langToDisplayString = new(){
            {"Japanese", ("日本語", Radzen.BadgeStyle.Info)}, {"English", ("英語", Radzen.BadgeStyle.Danger)}
        };
        public string GetLanguageScreenName() => langToDisplayString.ContainsKey(Language) ? langToDisplayString[Language].screenName : DefaultScreenName;
        public Radzen.BadgeStyle GetLanguageBadgeStyle() => langToDisplayString.ContainsKey(Language) ? langToDisplayString[Language].badgeStyle : DefaultBadgeStyle;


        public string? WorkId { get; set; }

        public string? Title { get; set; }

        public string? TitleYomi { get; set; }

        public string? SubTitle { get; set; }

        public string? Author { get; set; }

        public string? AuthorYomi { get; set; }

        public string? Language { get; set; }

        public string? Source { get; set; }

        public Uri? WorkUri { get; set; }
    }
}
