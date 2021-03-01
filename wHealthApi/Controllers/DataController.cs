using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wHealthApi.Models;
using wHealthApi.Models.JoinModels;

namespace wHealthApi.Controllers
{
    public class DataController : BaseController
    {

        private readonly wHealthappDbContext _context;
        public DataController(wHealthappDbContext context)
        {
            _context = context;
        }



        //RETURNING ALL THE ACTIVE STATUS CLINICS DATA
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                Response res = new Response();
                IList<Clinic> ClinicList = _context.Clinics.ToList();
                var query = (from c in _context.Clinics
                            join u in _context.Users
                            on c.Id equals u.ClinicId
                            where u.Status == "Active" 
                            select new { c.Email, c.Name, c.PhoneNo, c.RegistrationNo, c.Id, c.Address }).ToList();

                res.Status = true;
                res.Result = query;
                res.Message = "FOLLOWING CLINICS AVAILABLE";
                return Ok(res);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        //RETURNING A SPECIFIC CLINICS DATA
        [HttpPut]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            try
            {
                var DoctorData = _context.Doctors.Find(id);
                if (DoctorData != null)
                return Ok(DoctorData);

                var PatientData = _context.Patients.Find(id);
                if(PatientData!=null)
                return Ok(PatientData);

                var ClinicData = _context.Clinics.Find(id);
                if(ClinicData!=null)
                return Ok(ClinicData);

                return Ok("id not found");
            }
            catch (Exception ex)
            {

                throw;
            }
        }




    }
}




    }
}



