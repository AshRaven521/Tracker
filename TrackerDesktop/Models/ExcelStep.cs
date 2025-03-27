namespace TrackerDesktop.Models
{
    public class ExcelStep
    {
        public int Id { get; set; }
        public int ModeId { get; set; }
        public int Timer { get; set; }
        public string Destination { get; set; } = string.Empty;
        public int Speed { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Volume { get; set; }
    }
}
