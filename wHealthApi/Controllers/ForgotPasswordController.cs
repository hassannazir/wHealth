using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using wHealthApi.Helpers;
using wHealthApi.Models;

namespace wHealthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController :BaseController
    {

        private readonly wHealthappDbContext _context;
        public ForgotPasswordController(wHealthappDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SendCodeToEmail(int id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            Response response = new Response();

            if (user != null)
            {
                if (user.Type == AppConstants.Patient)
                {
                    var uPatient = _context.Patients.Where(u => u.Id == id).FirstOrDefault();

                    MailMessage em = new MailMessage();
                    em.To.Add(uPatient.Email);
                    em.From = new MailAddress("dev@theta.solutions", "wHealth");
                    em.Subject = "6 DIGIT CODE FOR FORGOT PASSWORD.";

                    Random generator = new Random();
                    int r = generator.Next(100000, 1000000);

                    em.Body = "<h4>Dear " + uPatient.Name + ", </h4><br>Please enter this six digit code in order to change password.<br><br>" + r + " <br><br>  Thanks.";

                    em.IsBodyHtml = true;

                    SmtpClient cli = new SmtpClient();
                    cli.Host = "116.202.175.92";
                    cli.Port = 25;
                    cli.UseDefaultCredentials = false;
                    cli.Credentials = new NetworkCredential("dev@theta.solutions", "8xTc$NU?%5kCh.5L&,u#:o^S|oCp");
                    cli.EnableSsl = false;

                    await cli.SendMailAsync(em);


                    response.Result = r;
                    response.Status = true;
                    return Ok(response);

                }
                else if (user.Type == AppConstants.Doctor)
                {
                    var uDoctor = _context.Doctors.Where(u => u.Id == id).FirstOrDefault();

                    MailMessage em = new MailMessage();
                    em.To.Add(uDoctor.Email);
                    em.From = new MailAddress("dev@theta.solutions", "wHealth");
                    em.Subject = "6 DIGIT CODE FOR FORGOT PASSWORD.";

                    Random generator = new Random();
                    int r = generator.Next(100000, 1000000);

                    em.Body = "<h4>Dear " + uDoctor.Name + ", </h4><br>Please enter this six digit code in order to change password.<br><br>" + r + " <br><br>  Thanks.";

                    em.IsBodyHtml = true;

                    SmtpClient cli = new SmtpClient();
                    cli.Host = "116.202.175.92";
                    cli.Port = 25;
                    cli.UseDefaultCredentials = false;
                    cli.Credentials = new NetworkCredential("dev@theta.solutions", "8xTc$NU?%5kCh.5L&,u#:o^S|oCp");
                    cli.EnableSsl = false;

                    await cli.SendMailAsync(em);


                    response.Result = r;
                    response.Status = true;
                    return Ok(response);
                }
                else if (user.Type == AppConstants.Clinic)
                {
                    var uClinic = _context.Clinics.Where(u => u.Id == id).FirstOrDefault();

                    MailMessage em = new MailMessage();
                    em.To.Add(uClinic.Email);
                    em.From = new MailAddress("dev@theta.solutions", "wHealth");
                    em.Subject = "6 DIGIT CODE FOR FORGOT PASSWORD.";

                    Random generator = new Random();
                    int r = generator.Next(100000, 1000000);

                    em.Body = "<h4>Dear " + uClinic.Name + ", </h4><br>Please enter this six digit code in order to change password.<br><br>" + r + " <br><br>  Thanks.";

                    em.IsBodyHtml = true;

                    SmtpClient cli = new SmtpClient();
                    cli.Host = "116.202.175.92";
                    cli.Port = 25;
                    cli.UseDefaultCredentials = false;
                    cli.Credentials = new NetworkCredential("dev@theta.solutions", "8xTc$NU?%5kCh.5L&,u#:o^S|oCp");
                    cli.EnableSsl = false;

                    await cli.SendMailAsync(em);


                    response.Result = r;
                    response.Status = true;
                    return Ok(response);

                }
            }


                response.Result = 0;
                return Ok(response);

            


        }

        [HttpPut]
        public async Task<IActionResult> SaveUpdatedPassword(int id,string newpass)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            Response response = new Response();
            if (user!=null)
            {
                user.Password = newpass;
                response.Status = true;
                response.Result = "Your password Updated Successfully";
                return Ok(response);
                
            }
            response.Status = false;
            return Ok(response);

        }
    }
}
