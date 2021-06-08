using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using wHealthApi.Models;

namespace wHealthApi.Controllers
{
    public class BookingsController : BaseController
    {

        private readonly wHealthappDbContext _context;
        public BookingsController(wHealthappDbContext context)
        {
            _context = context;
        }




        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> BookAppointmentAsync(int patientId,int doctorId,int clinicId,int status,TimeSpan startTime, TimeSpan endTime, DateTime date)
        {
            Response response = new Response();
            Appointment ap = new Appointment();
            ap.PatientId = patientId;
            ap.DoctorId = doctorId;
            ap.ClinicId = clinicId;
            ap.Status = status;
            ap.StartTime = startTime;
            ap.Date = date;
            
            ap.EndTime = endTime;


            try
            {

                IList<Schedule> schedules = _context.Schedules.Where(i => i.DoctorId == doctorId && i.ClinicId==clinicId).ToList();
                IList<Appointment> appointments = _context.Appointments.ToList();
                if (schedules.Count() > 0)
                {
                    foreach (Schedule s in schedules)
                    {
                        if (date >= s.StartDate && date <= s.EndDate)
                        {
                            foreach (Appointment a in appointments)
                            {
                                if (date == a.Date)
                                {
                                    if (a.StartTime >= a.StartTime && a.EndTime <= a.EndTime)
                                    {
                                        response.Message = "This slot is already booked";
                                        response.Status = false;
                                        response.Result = null;
                                        return Ok(response);
                                    }
                                }
                            }
                            if ((startTime >= s.StartTime && startTime < s.EndTime) && (endTime > s.StartTime && endTime <= s.EndTime))
                            {


                                await _context.Appointments.AddAsync(ap);
                                await _context.SaveChangesAsync();

                                //var patient = _context.Patients.Where(a => a.Id == patientId).FirstOrDefault();
                                //var doc = _context.Doctors.Where(a => a.Id == doctorId).FirstOrDefault();

                                //var clinic = _context.Clinics.Where(a => a.Id == clinicId).FirstOrDefault();

                                //MailMessage em = new MailMessage();
                                //em.To.Add(patient.Email);
                                //em.From = new MailAddress("faizanmunirofficial@gmail.com", "wHealth");
                                //em.Subject = "--Appointment Notification--";



                                //em.Body = "<h4>Dear " + patient.Name + ", </h4><br>Your Appointment has been booked with.<br><br>" + doc.Name + " in " + clinic.Name + "<br><br>  Thanks.";

                                //em.IsBodyHtml = true;

                                //SmtpClient cli = new SmtpClient();
                                //cli.Host = "116.202.175.92";
                                //cli.Port = 25;
                                //cli.UseDefaultCredentials = false;
                                //cli.Credentials = new NetworkCredential("faizanmunirofficial@gmail.com", "saim9797");
                                //cli.EnableSsl = false;

                                //await cli.SendMailAsync(em);


                                response.Message = "Appointmnt is booked!";
                                response.Status = true;
                                return Ok(response);
                            }
                            else
                            {
                                response.Message = "This time slot is not available";
                                response.Status = false;
                                return Ok(response);
                            }

                        }
                        



                    }

                    response.Message = "Doctor is not available in this clinic on this day";
                    response.Status = false;
                    response.Result = null;
                    return Ok(response);

                }
                else
                {
                    response.Message = "this doctor has no schedule in this clinic";
                    response.Status = false;
                    response.Result = schedules;
                }
                response.Message = "Doctor is not available in this clinic on this day";
                response.Status = false;
                response.Result = null;
                return Ok(response);

            }
            catch(Exception ex) 
            {
                throw;
            }
              


        }
        [HttpGet]
        [AllowAnonymous]
        public  IActionResult GetAppointments()
        {
            Response response = new Response();
            IList<Appointment> appointmentList = _context.Appointments.ToList();
            if (appointmentList.Count() == 0)
            {
                

                response.Message = "No appointmets available ";
                response.Result = appointmentList;
                response.Status = false;
                return Ok(response);
            }
            else 
            {
                response.Message = "List of all appointments";
                response.Result = appointmentList;
                response.Status = true;
                return Ok(response);

            }

        }



    }
}
