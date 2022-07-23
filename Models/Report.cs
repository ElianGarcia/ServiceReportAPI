using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceReportAPI.Models
{
    public class Report
    {
        public Int64 ReportId { get; set; }
        public decimal Hours { get; set; }
        public int Videos { get; set; }
        public int Placements { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public Int64 UserId { get; set; }
        public User? User { get; set; }

        public Report()
        {
            ReportId = 0;
            Hours = 0;
            Videos = 0;
            Placements = 0;
            User = new User();
        }

        public Report(int iD, decimal hours, int videos, int placements, DateTime date, long userId, User user)
        {
            ReportId = iD;
            Hours = hours;
            Videos = videos;
            Placements = placements;
            Date = date;
            UserId = userId;
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
