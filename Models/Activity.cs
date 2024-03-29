﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceReportAPI.Models
{
    public class Activity
    {
        public Int64 ActivityId { get; set; }
        public decimal Hours { get; set; }
        public int Videos { get; set; }
        public int Placements { get; set; }
        public int ReturnVisits { get; set; }
        public DateTime Date { get; set; }
        public Int64 UserId { get; set; }

        public Activity()
        {
            ActivityId = 0;
            Hours = 0;
            Videos = 0;
            Placements = 0;
            Date = DateTime.Now;
        }
    }
}
