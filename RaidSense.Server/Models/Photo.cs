namespace RaidSense.Server.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public int BaseId { get; set; }
        public Base Base { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
