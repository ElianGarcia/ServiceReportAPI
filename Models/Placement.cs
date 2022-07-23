namespace ServiceReportAPI.Models
{
    public class Placement
    {
        public int PlacementId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public Placement()
        {
            Name = String.Empty;
            ShortName = String.Empty;  
        }

        public Placement(int iD, string name, string shortName)
        {
            PlacementId = iD;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
        }
    }
}
