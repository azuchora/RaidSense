namespace RaidSense.Server.Models.Battlemetrics
{

    public class BmPlayerResponseDto
    {
        public Data? Data { get; set; }
    }

    public class Data
    {
        public string Type { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public Attributes Attributes { get; set; } = new();
    }

    public class Attributes
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool Private { get; set; }
        public bool PositiveMatch { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}