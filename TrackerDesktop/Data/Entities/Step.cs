namespace TrackerDesktop.Data.Entities
{
    public class Step
    {
        public int Id { get; set; }
        public int ModeId { get; set; }
        public int Timer { get; set; }
        public string? Destination { get; set; }
        public int Speed { get; set; }
        public string Type { get; set; }
        public int Volume { get; set; }
        public Mode Mode { get; set; } = new Mode();

    }
}
