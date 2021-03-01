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

        [HttpPut]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            try
            {
                Response res = new Response();

                var ClinicData = _context.Clinics.Find(id);
                res.Message = "Selected Clinic's data.";
                res.Result = ClinicData;
                res.Status = true;
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw;
            }
        }




    }
}



