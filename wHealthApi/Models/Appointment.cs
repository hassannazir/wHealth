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
        public string Status { get; set; }
        public DateTime? Slot { get; set; }
        public string DetailFromPatient { get; set; }
        public string Diagnose { get; set; }
        public string Prescription { get; set; }
        public string Remarks { get; set; }
        public string Complain { get; set; }
    }
}
