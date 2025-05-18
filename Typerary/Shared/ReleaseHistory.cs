using System.Text.Json.Serialization;

namespace Typerary.Shared
{
    public class ReleaseHistoryMetadata
    {
        [JsonInclude]
        public List<ReleaseHistory>? Histories { get; set; }
    }

    public class ReleaseHistory
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
        public string? UpdateDate { get; set; }
        
        [JsonInclude]
        public string? Version { get; set; }
        
        [JsonInclude]
        public string? DetailText { get; set; }
    }
}
