using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListOfPatientAppointments(int patientId)
        {
            Response res = new Response();

            try
            {
                var query = (from p in _context.Patients
                             join app in _context.Appointments
                             on p.Id equals app.PatientId
                             join c in _context.Clinics
                             on app.ClinicId equals c.Id
                             where app.PatientId == patientId
                             select new { patientName = p.Name, clinicName = c.Name, app.Status , startTime = app.StartTime.ToString(), endTime=app.EndTime.ToString(), app.Date }).ToList();
           

                if (query != null)
                {
                    res.Status = true;
                    res.Result = query;
                    res.Message = "Patients Appointmenst";
                    return Ok(res);
                }
                else
                {
                    res.Status = false;
                    res.Message = "Patients Booking List is Empty";
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = "Error";
                return Ok(res);
            }


 
            

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> BookAppointmentAsync(int patientId, int doctorId, int clinicId, int status, TimeSpan startTime, TimeSpan endTime, DateTime date)
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

                IList<Schedule> schedules = _context.Schedules.Where(i => i.DoctorId == doctorId && i.ClinicId == clinicId).ToList();
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
                                    if ((startTime >= a.StartTime && startTime <= a.EndTime) || (endTime >= a.StartTime && endTime <= a.EndTime) || (startTime <= a.StartTime && endTime >= a.EndTime))
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
            catch (Exception ex)
            {
                throw;
            }



        }

        [HttpGet]
        [AllowAnonymous]
        public  IActionResult GetPendingOrBookedAppointments(int doctorId,int clinicId)
        {
            Response response = new Response();
            IList<Appointment> appointmentList = _context.Appointments.Where(i => i.DoctorId == doctorId && i.ClinicId == clinicId && i.Status!=0).ToList();
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetBookedAppointments(int doctorId)
        {
            Response response = new Response();
            IList<Appointment> appointmentList = _context.Appointments.Where(i => i.DoctorId == doctorId && i.Status == 2).ToList();
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

        [HttpGet]
        [AllowAnonymous]
        public  IActionResult PatientsBookingRequests(int doc_id)
        {
            Response res = new Response();
            try
            {
              
                var query =  (from p in _context.Patients
                                   join app in _context.Appointments
                                   on p.Id equals app.PatientId
                                   join c in _context.Clinics
                                   on app.ClinicId equals c.Id
                                   where app.DoctorId == doc_id && app.Status == 1
                                   select new { appointmentId = app.Id, patientName = p.Name, clinicName = c.Name, startTime=app.StartTime.ToString(), endTime=app.EndTime.ToString(), app.Date }).ToList();

                if (query!=null)
                {
                    res.Status = true;
                    res.Result = query;
                    res.Message = "Patients Booking Requests..";
                    return Ok(res);
                }
                else
                {
                    res.Status = false;
                    res.Message = "Patients Booking List is Empty";
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = "Error Occurs..";
                return Ok(res);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ApproveOrCancelPatientRequest(int appointmentId,bool status)
        {
            Response res = new Response();
            try
            {
                Appointment appointment = await _context.Appointments.FindAsync(appointmentId);
                if (appointment == null)
                {
                        res.Status = false;
                        res.Message = "Appointment not found";
                        return Ok(res);
                }
                if (status)
                {

                    appointment.Status = 2;
                    _context.Appointments.Update(appointment);
                    await _context.SaveChangesAsync();
                    res.Status = true;
                    res.Message = "Booking Confirmed";
                    return Ok(res);
                }
                else
                {
                    appointment.Status = 0;
                    _context.Appointments.Update(appointment);
                    await _context.SaveChangesAsync();
                    res.Status = true;
                    res.Message = "Booking Cancelled";
                    return Ok(res);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }




    }
}
