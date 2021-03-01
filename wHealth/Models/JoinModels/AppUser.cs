using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wHealth.Models.JoinModels
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public string RegistrationNo { get; set; }
        public string LicenseNo { get; set; }
        public string Qualification { get; set; }
        public string Experience { get; set; }
    }
}
