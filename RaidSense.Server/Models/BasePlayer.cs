namespace RaidSense.Server.Models
{
    public class BasePlayer
    {
        public int BaseId { get; set; }
        public Base Base { get; set; } = null!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}
