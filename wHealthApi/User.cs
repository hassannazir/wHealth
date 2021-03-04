using System;
using System.Collections.Generic;

#nullable disable

namespace wHealthApi
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public int? ClinicId { get; set; }
    }
}
