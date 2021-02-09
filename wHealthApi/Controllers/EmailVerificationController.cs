using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wHealthApi.Models;

namespace wHealthApi.Controllers
{
    public class EmailVerificationController :BaseController
    {      

        private readonly wHealthappDbContext _context;
        public EmailVerificationController(wHealthappDbContext context)
        {
            _context = context;
        }
      

       
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAsync(int id)
        {
            var us = await _context.Users.FindAsync(id);

            if (us != null)
            {
                us.Status = "Active";
                await _context.SaveChangesAsync();
            }

            return Ok();
        }



        


    }
}
