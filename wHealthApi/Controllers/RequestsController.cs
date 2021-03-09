using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using wHealthApi.Models;
using wHealthApi.Models.JoinModels;

namespace wHealthApi.Controllers
{
    public class RequestsController : BaseController
    {

        private readonly wHealthappDbContext _context;
        public RequestsController(wHealthappDbContext context)
        {
            _context = context;
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> DoctorClinicApprovalAsync(int cid, int did)
        {

            try
            {
                var DocWhoWillWork = _context.Doctorclinics.Where(e => e.ClinicId == cid && e.DoctorId == did).FirstOrDefault();
                var doc = _context.Doctors.Where(e => e.Id == did).FirstOrDefault();
                var clin = _context.Clinics.Where(e => e.Id == cid).FirstOrDefault();

                DocWhoWillWork.Status = "Active";  //DOCTOR WILL GET TO WORK IN THE SPECIFIED CLINIC

                _context.SaveChanges();
                MailMessage em = new MailMessage();
                em.To.Add(doc.Email);
                em.From = new MailAddress("dev@theta.solutions", "wHealth");
                em.Subject = "REQUEST TO WORK IN CLINIC";

                // em.Body = "<h4>Dear " + appUser.Name + ", </h4><br>Please click on the following link to confirm your email.<br><br>" + "<a href='https://localhost:44340/Id?Id=" + user.Username + "'>CLICK HERE! </a><br><br>  Thanks.";
                em.Body = "<h4>Dear " + doc.Name + ", </h4><br>YOUR REQUEST TO WORK FOR THE CLINIC " + clin.Name + " ,  YOU SELECTED HAS BEEN APPROVED. NOW YOU CAN WORK IN THE CLINIC. CONGRATULATIONS!!!!<br><br>";

                em.IsBodyHtml = true;

                SmtpClient cli = new SmtpClient();
                cli.Host = "116.202.175.92";
                cli.Port = 25;

                cli.UseDefaultCredentials = false;
                cli.Credentials = new NetworkCredential("dev@theta.solutions", "8xTc$NU?%5kCh.5L&,u#:o^S|oCp");
                cli.EnableSsl = false;
                await cli.SendMailAsync(em);
                Response r = new Response();
                r.Status = true;
                r.Message = "You approved the request, Successfully.";
                return Ok(r);


            }
            catch
            {
                throw;
            }

        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult DoctorSendRequestToClinic(int docId, int clinicId)
        {
            Response response = new Response();
            Doctorclinic docClinicData = new Doctorclinic();
            docClinicData.ClinicId = clinicId;
            docClinicData.DoctorId = docId;
            docClinicData.Status = "InActive";
            try
            {
                _context.Doctorclinics.Add(docClinicData);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            response.Message = "Your request has been SUBMITTED.And you will be informed if clinic APPROVED your request";
            return Ok(response);
        }


        [HttpDelete]
        [AllowAnonymous]
        public IActionResult ViewDocSchedule(int docId)
        {
            try
            {
                Response res = new Response();
                
                var query = (from c in _context.Schedules
                             join u in _context.Doctors
                             on c.DoctorId equals u.Id
                             where c.DoctorId == docId
                             select new { c.StartTime, c.EndTime, c.ClinicId }).ToList();
                if (!query.Any())
                {
                    res.Status = false;

                    res.Result = query;
                    res.Message = "this doctor has not set any Schedule";
                    return Ok(res);
                }
                res.Status = true;

                res.Result = query;
                res.Message = "Here's the doctor's Schedule";
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        

    }
}




