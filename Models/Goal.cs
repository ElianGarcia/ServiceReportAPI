using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceReportAPI.Models
{
    public class Goal
    {
        [Key]
        public Int64 GoalId { get; set; }
        public decimal Hours { get; set; }
        public int Placements { get; set; }
        public int Videos { get; set; }
        public int Students { get; set; }
        public Int64? UserId { get; set; }
        public Int64? ReturnVisits { get; set; }
        public int? Month { get; set; }
        public Goal? Progress { get; set; }

        public Goal()
        {
            Hours = 0;
            Placements = 0;
            Videos = 0;
            Students = 0;
            UserId = 0;
        }

        public Goal(int id, decimal hours, int placements, int videos, Int64 returnVisits, long userId)
        {
            GoalId = id;
            Hours = hours;
            Placements = placements;
            Videos = videos;
            ReturnVisits = returnVisits;
            UserId = userId;
        }

        public Goal(int id, decimal hours, int placements, int videos, Int64 returnVisits, long userId, int? month)
        {
            GoalId = id;
            Hours = hours;
            Placements = placements;
            Videos = videos;
            UserId = userId;
            ReturnVisits = returnVisits;
            Month = month;
        }
    }
}
