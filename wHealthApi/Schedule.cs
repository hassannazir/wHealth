using System;
using System.Collections.Generic;

#nullable disable

namespace wHealthApi
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? DoctorId { get; set; }
        public int? ClinicId { get; set; }
    }
}
