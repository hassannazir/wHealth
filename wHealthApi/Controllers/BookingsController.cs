﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> BookAppointmentAsync(int patientId,int doctorId,int clinicId,int status,TimeSpan startTime,  DateTime date,int scheduleId, TimeSpan endTime)
        {


            try
            {
                Response response = new Response();
                Schedule s = await _context.Schedules.FindAsync(scheduleId);
                Appointment a = new Appointment();
                a.PatientId = patientId;
                a.DoctorId = doctorId;
                a.ClinicId = clinicId;
                a.Status = status;
                a.StartTime = startTime;
                a.Date = date;
                a.ScheduleId = scheduleId;
                a.EndTime = endTime;


                IList<Appointment> appointments =  _context.Appointments.ToList();

                if (a.Date >= s.StartDate && a.Date <= s.EndDate)
                {
                    foreach (Appointment appointment in appointments)
                    {
                        if (a.Date == appointment.Date)
                        {
                            if (a.StartTime >= appointment.StartTime && a.EndTime <= appointment.EndTime)
                            {
                                response.Message = "This time slot is already booked";
                                response.Status = false;
                                return Ok(response);
                            }
                        }
                    }

                    if ((a.StartTime >= s.StartTime && a.StartTime < s.EndTime) && (a.EndTime > s.StartTime && a.EndTime <= s.EndTime))
                    {
                       

                        await _context.Appointments.AddAsync(a);
                        await _context.SaveChangesAsync();

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
                else
                {
                    response.Message = "Doctor is not available on this day!";
                    response.Status = false;
                    return Ok(response);
                }
                


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