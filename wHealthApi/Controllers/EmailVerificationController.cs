using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wHealthApi.Models;


namespace wHealthApi.Controllers
{
    public class EmailVerificationController : Controller
    {

        private readonly wHealthappDbContext _context;
        public EmailVerificationController(wHealthappDbContext context)
        {
            _context = context;
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



        [HttpGet("userName")]
        [AllowAnonymous]
        public IActionResult Verify(String userName)
        {


            var us = _context.Users.Where(u => u.Username == userName).FirstOrDefault();



            if (us != null)
            {

                us.Status = "Active";
                _context.SaveChanges();
                return Ok("verification success!!!! and now you can login the app......");

            }

            return Ok("VERFICATION FAILED!!!");




        }


    }
}
