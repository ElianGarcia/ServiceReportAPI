using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceReportAPI.Models
{
    public class Student
    {
        public Int64 StudentId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }
        public int DayToVisit { get; set; }
        public string Observations { get; set; }
        public long UserId { get; set; }
        public long PlacementId { get; set; }
        public string? PlacementName { get; set; }

        public Student()
        {
            Name = String.Empty;
            Address = String.Empty;
            Phone = String.Empty;
            PlacementId = 0;
            Active = true;
            DayToVisit = 0;
            Observations = String.Empty;
            UserId = 0;
        }

        public Student(long iD, string name, string address, string phone, int placementId, bool active, int dayToVisit, string observations, long userId)
        {
            StudentId = iD;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            PlacementId = placementId;
            Active = active;
            DayToVisit = dayToVisit;
            Observations = observations ?? throw new ArgumentNullException(nameof(observations));
            UserId = userId;
        }
    }
}
