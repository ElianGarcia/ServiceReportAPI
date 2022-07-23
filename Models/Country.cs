namespace ServiceReportAPI.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }

        public Country()
        {
        }

        public Country(int countryId, string name)
        {
            CountryId = countryId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
