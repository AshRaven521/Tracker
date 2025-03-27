using System.Collections.Generic;

namespace TrackerDesktop.Data.Entities
{
    public class Step
    {
        public int Id { get; set; }
        public int ModeId { get; set; }
        public int Timer { get; set; }
        public string Destination { get; set; } = string.Empty;
        public int Speed { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Volume { get; set; }
        public Mode Mode { get; set; } = new Mode();
        public User? User { get; set; }

        public override bool Equals(object? obj)
        {
            var item = obj as Step;
            if (item == null)
            {
                return false;
            }

            return (this.Id == item.Id)
                && (this.ModeId == item.ModeId)
                && (this.Timer == item.Timer)
                && (this.Destination == item.Destination)
                && (this.Speed == item.Speed)
                && (this.Type == item.Type)
                && (this.Volume == item.Volume);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^
                ModeId.GetHashCode() ^
                Timer.GetHashCode() ^
                Destination.GetHashCode() ^
                Speed.GetHashCode() ^
                Type.GetHashCode() ^
                Volume.GetHashCode();
        }

    }
}
