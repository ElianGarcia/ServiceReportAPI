﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceReportAPI.Models
{
    public class Goal
    {
        [Key]
        public Int64 GoalId { get; set; }
        public int Hours { get; set; }
        public int Placements { get; set; }
        public int Videos { get; set; }
        public Int64 UserID { get; set; }

        public Goal()
        {
            Hours = 0;
            Placements = 0;
            Videos = 0;
            UserID = 0;
        }

        public Goal(int id, int hours, int placements, int videos, long userId)
        {
            GoalId = id;
            Hours = hours;
            Placements = placements;
            Videos = videos;
            UserID = userId;
        }
    }
}