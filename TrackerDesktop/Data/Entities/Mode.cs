using System.Collections.Generic;

namespace TrackerDesktop.Data.Entities
{
    public class Mode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxBottleNumber { get; set; }
        public int MaxUsedTips { get; set; }
        public List<User?> Users { get; set; } = new List<User?>();

        public override bool Equals(object? obj)
        {
            var item = obj as Mode;
            if (item == null)
            {
                return false;
            }

            return (this.Id == item.Id)
                && (this.Name == item.Name)
                && (this.MaxBottleNumber == item.MaxBottleNumber)
                && (this.MaxUsedTips == item.MaxUsedTips);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^
                Name.GetHashCode() ^
                MaxBottleNumber.GetHashCode() ^
                MaxUsedTips.GetHashCode();
        }
    }
}
