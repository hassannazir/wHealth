using System;
using System.Collections.Generic;

#nullable disable

namespace wHealth
{
    public partial class Doctorclinic
    {
        public int Id { get; set; }
        public int? DoctorId { get; set; }
        public int? ClinicId { get; set; }
        public string Status { get; set; }
    }
}
