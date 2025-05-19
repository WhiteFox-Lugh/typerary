using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public record ReleaseHistoryMetadata
    {
        [JsonInclude]
        public List<ReleaseHistory>? Histories { get; init; }
    }

    public record ReleaseHistory
    {
        public DateTime GetUpdateDateTime()
        {
            if (!DateTime.TryParse(UpdateDate, out var res))
            {
                throw new InvalidDataException("Error: invalid datetime format in release history");
            }
            return res;
        }

        [JsonInclude]
        public string? UpdateDate { get; init; }
        
        [JsonInclude]
        public string? Version { get; init; }
        
        [JsonInclude]
        public string? DetailText { get; init; }
    }
}
