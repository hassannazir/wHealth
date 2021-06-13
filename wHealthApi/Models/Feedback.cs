using System;
using System.Collections.Generic;

#nullable disable

namespace wHealthApi.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? ClinicId { get; set; }
        public float Rating { get; set; }
        public string Review { get; set; }
    }
}
