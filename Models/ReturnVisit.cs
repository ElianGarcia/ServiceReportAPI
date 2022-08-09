namespace ServiceReportAPI.Models
{
    public class ReturnVisit
    {
        public Int64 VisitId { get; set; }
        public Int64 StudentId { get; set; }
        public Int64 UserId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
    }
}
