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
    public class PasswordHandlerController :BaseController
    {
        private readonly wHealthappDbContext _context;
        public PasswordHandlerController(wHealthappDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SendCodeToEmail(string usrname)
        {
            var user = _context.Users.Where(u => u.Username == usrname).FirstOrDefault();
            Response response = new Response();

            if (user != null)
            {
                if (user.Type == AppConstants.Patient)
                {
                    var uPatient = _context.Patients.Where(u => u.Id ==user.PatientId).FirstOrDefault();

                    MailMessage em = new MailMessage();
                    em.To.Add(uPatient.Email);
                    em.From = new MailAddress("dev@theta.solutions", "wHealth");
                    em.Subject = "--  PASSWORD RECOVERY CODE  --";

                    Random generator = new Random();
                    int r = generator.Next(100000, 1000000);

                    em.Body = "<h4>Dear " + uPatient.Name + ", </h4><br>Please enter this six digit code in the app in order to change password.<br><br>" + r + " <br><br>  Thanks.";

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
                    var uDoctor = _context.Doctors.Where(u => u.Id ==user.DoctorId).FirstOrDefault();

                    MailMessage em = new MailMessage();
                    em.To.Add(uDoctor.Email);
                    em.From = new MailAddress("dev@theta.solutions", "wHealth");
                    em.Subject = "--  PASSWORD RECOVERY CODE  --";


                    Random generator = new Random();
                    int r = generator.Next(100000, 1000000);

                    em.Body = "<h4>Dear " + uDoctor.Name + ", </h4><br>Please enter this six digit code in the app in order to change password.<br><br>" + r + " <br><br>  Thanks.";

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
                    var uClinic = _context.Clinics.Where(u => u.Id == user.ClinicId).FirstOrDefault();

                    MailMessage em = new MailMessage();
                    em.To.Add(uClinic.Email);
                    em.From = new MailAddress("dev@theta.solutions", "wHealth");
                    em.Subject = "--  PASSWORD RECOVERY CODE  --";

                    Random generator = new Random();
                    int r = generator.Next(100000, 1000000);

                    em.Body = "<h4>Dear " + uClinic.Name + ", </h4><br>Please enter this six digit code in the app in order to change password.<br><br>" + r + " <br><br>  Thanks.";

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


                response.Result ="This username does not exist in the system";
                return Ok(response);

            


        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> SaveUpdatedPassword(string usrname,string newpass)
        {
            var user = _context.Users.Where(u => u.Username == usrname).FirstOrDefault();
            Response response = new Response();
            if (user!=null)
            {
                try
                {
                    user.Password = newpass;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    throw;
                }
               
                response.Status = true;
                response.Result = "Your password Updated Successfully";
                return Ok(response);
                
            }
            response.Status = false;
            response.Result = "This username does not exist in the system";
            return Ok(response);

        }
    }
}
