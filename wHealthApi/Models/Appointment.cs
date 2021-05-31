using System;
using System.Collections.Generic;

#nullable disable

namespace wHealthApi.Models
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? ClinicId { get; set; }
        public int? Status { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? Date { get; set; }
        
        public int ScheduleId { get;  set; }
        public TimeSpan EndTime { get; set; }

    }
}
