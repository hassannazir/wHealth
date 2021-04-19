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
            catch (Exception )
            {
                throw;
            }

            response.Message = "Your request has been SUBMITTED.And you will be informed if clinic APPROVED your request";
            response.Status = true;
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> setDocSchedule(int doctorId, int clinicId, TimeSpan startTime,TimeSpan endTime,DateTime startDate, DateTime endDate, bool recurring, string day)
        {
            try
            {

                Response res = new Response();
                IList<Schedule> ilist = _context.Schedules.Where(sc => sc.DoctorId == doctorId).ToList();
                Schedule schedule = new Schedule();

                if (ilist != null) {

                    foreach (Schedule s in ilist)
                    {
                        if (s.Day == day && recurring)
                        {
                            if ((startDate >= s.StartDate && endDate <= s.EndDate) || (startDate >= s.StartDate && startDate <= s.EndDate) || (endDate >= s.StartDate && endDate <= s.EndDate) || (startDate <= s.StartDate && endDate >= s.EndDate))
                            {
                                if ((startTime >= s.StartTime && endTime <= s.EndTime) || (startTime >= s.StartTime && startTime <= s.EndTime) || (endTime >= s.StartTime && endTime <= s.EndTime) || (startTime <= s.StartTime && endTime >= s.EndTime))
                                {
                                    if (s.ClinicId != clinicId)
                                    {
                                        res.Status = false;
                                        res.Result = null;
                                        res.Message = "This time slot is not free. Please select another time slot.";
                                        return Ok(res);
                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            if ((startDate >= s.StartDate && endDate <= s.EndDate) || (startDate >= s.StartDate && startDate <= s.EndDate) || (endDate >= s.StartDate && endDate <= s.EndDate) || (startDate <= s.StartDate && endDate >= s.EndDate))
                            {
                                if ((startTime >= s.StartTime && endTime <= s.EndTime) || (startTime >= s.StartTime && startTime <= s.EndTime) || (endTime >= s.StartTime && endTime <= s.EndTime) || (startTime <= s.StartTime && endTime >= s.EndTime))
                                {
                                    if (s.ClinicId != clinicId)
                                    {
                                        res.Status = false;
                                        res.Result = null;
                                        res.Message = "This time slot is not free. Please select another time slot.";
                                        return Ok(res);
                                    }
                                }
                            }
                        }

                    }
                }

                    schedule.StartTime = startTime;
                    schedule.EndTime = endTime;
                    schedule.DoctorId = doctorId;
                    schedule.ClinicId = clinicId;
                    schedule.StartDate = startDate;
                    schedule.EndDate = endDate;
                    schedule.Day = day;
                    schedule.Recurring = recurring;

                    await _context.Schedules.AddAsync(schedule);
                    await _context.SaveChangesAsync();

                    res.Status = true;

                    res.Message = "Time slot set seccessfully.";

                    return Ok(res);

               
            }
            
            catch (Exception )
            {

                throw;
            }
        }
        
        [HttpPost]
        [AllowAnonymous]
        public IActionResult clincsOfLoggoedInDoctors(int doc_id)
        {
            try
            {
                Response res = new Response();
                IList<Clinic> ClinicList = _context.Clinics.ToList();
                var query = (from dc in _context.Doctorclinics
                             join c in _context.Clinics
                             on dc.ClinicId equals c.Id
                             where dc.DoctorId ==doc_id && dc.Status=="Active"
                             select new { c.Name, c.Email, c.Address, c.PhoneNo,c.RegistrationNo,c.Id }).ToList();
                res.Status = true;
                res.Result = query;
                res.Message = "YOU ARE WORKING IN FOLLOWING CLINICS";
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw;
            }


        }


    }
}




