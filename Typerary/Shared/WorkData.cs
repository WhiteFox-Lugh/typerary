using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public record WorkDataList
    {
        [JsonInclude]
        public List<WorkData>? WorkList { get; init; }
    }

    public record WorkData
    {
        private const string DefaultScreenName = "---";
        private const Radzen.BadgeStyle DefaultBadgeStyle = Radzen.BadgeStyle.Dark;
        private static readonly Dictionary<string, (string screenName, Radzen.BadgeStyle badgeStyle)> LangToDisplayString = new()
        {
            { "Japanese", ("日本語", Radzen.BadgeStyle.Primary) },
            { "English", ("English", Radzen.BadgeStyle.Secondary) }
        };
        public string GetLanguageScreenName() => !string.IsNullOrEmpty(Language) && LangToDisplayString.ContainsKey(Language)
            ? LangToDisplayString[Language].screenName : DefaultScreenName;
        public Radzen.BadgeStyle GetLanguageBadgeStyle() => !string.IsNullOrEmpty(Language) && LangToDisplayString.ContainsKey(Language)
            ? LangToDisplayString[Language].badgeStyle : DefaultBadgeStyle;
        
        [JsonInclude]
        public string? WorkId { get; init; }

        [JsonInclude]
        public string? Title { get; init; }

        [JsonInclude]
        public string? TitleKana { get; init; }

        [JsonInclude]
        public string? SubTitle { get; init; }

        [JsonInclude]
        public string? Author { get; init; }

        [JsonInclude]
        public string? AuthorKana { get; init; }

        [JsonInclude]
        public string? Language { get; init; }

        [JsonInclude]
        public string? Source { get; init; }

        [JsonInclude]
        public Uri? WorkUri { get; init; }
    }
}
