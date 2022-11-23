namespace Typerary.Shared
{
    public class ReleaseHistoryMetadata
    {
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

        public string? UpdateDate { get; set; }
        public string? Version { get; set; }
        public string? DetailText { get; set; }
    }
}
