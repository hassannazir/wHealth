﻿using System;
using System.Collections.Generic;

#nullable disable

namespace wHealthApi.Models
{
    public partial class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string ProfilePic { get; set; }
    }
}
