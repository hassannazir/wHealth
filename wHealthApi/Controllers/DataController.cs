using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wHealthApi.Models;


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

                IList<Clinic> ClinicList = _context.Clinics.ToList();
                return Ok(ClinicList);
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
                var ClinicData = _context.Clinics.Find(id);

                return Ok(ClinicData);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}





//[HttpGet]
//[AllowAnonymous]
//public async Task<IActionResult> VerifyAsync(int id)
//{
//    var us = await _context.Users.FindAsync(id);

//    if (us != null)
//    {
//        us.Status = "Active";
//        await _context.SaveChangesAsync();
//    }

//    return Ok();
//}