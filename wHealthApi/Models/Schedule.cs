using System;
using System.Collections.Generic;

#nullable disable

namespace wHealthApi.Models
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public int DoctorId { get; set; }
        public int ClinicId { get; set; }

        public String Day { get; set; }
        public bool Recurring { get; set; }

        public int SlotLength { get; set; }




    }
}
