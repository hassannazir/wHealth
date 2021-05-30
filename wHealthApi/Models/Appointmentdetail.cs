using System;
using System.Collections.Generic;

#nullable disable

namespace wHealthApi.Models
{
    public partial class Appointmentdetail
    {
        public int Id { get; set; }
        public int? AppointmentId { get; set; }
        public string DetailFromPatient { get; set; }
        public string Diagnose { get; set; }
        public string Prescription { get; set; }
        public string Remarks { get; set; }
        public string Complain { get; set; }
    }
}
