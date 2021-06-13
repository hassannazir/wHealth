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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PatientFeedback(int patientId,int doctorId, int clinicId,float rating,string review)
        {
            Response res = new Response();
            Feedback fd = new Feedback();
            fd.PatientId = patientId;
            fd.DoctorId = doctorId;
            fd.ClinicId = clinicId;
            fd.Rating = rating;
            fd.Review = review;

            try
            {
                await _context.Feedbacks.AddAsync(fd);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = "Error occuring while updating data";
                return Ok(res);
            }

            res.Status = true;
            res.Message = "Review submitted successfully";
            return Ok(res);

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FeedbackList(int doctorId)
        {
            Response res = new Response();
            try
            {
                var query = (from p in  _context.Patients
                             join fd in _context.Feedbacks
                             on p.Id equals fd.PatientId
                             join c in _context.Clinics
                                  on fd.ClinicId equals c.Id
                             where fd.DoctorId == doctorId
                             select new { patientName = p.Name,clinicName=c.Name,fd.Review,rating=Convert.ToDouble(fd.Rating) }).ToList();


                if (query != null)
                {
                    res.Status = true;
                    res.Result = query;
                    res.Message = "Patient Review List About Doctor";
                    return Ok(res);
                }
               
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = "Error Occurs..";
                return Ok(res);
            }
            res.Status = false;
            res.Message = "Patients Review List is Empty";
            return Ok(res);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSlots(int doctorId, int clinicId, DateTime date)
        {
            Schedule schedule = new Schedule();
            Response res = new Response();

            List<Appointment> slotList = new List<Appointment>();
            var scheduleList = _context.Schedules.Where(d => d.DoctorId == doctorId && d.ClinicId == clinicId).ToList();

            for (int i = 0; i < scheduleList.Count; i++)
            {
                if (date >= scheduleList[i].StartDate && date <= scheduleList[i].EndDate && scheduleList[i].Recurring != true)
                {
                    schedule = scheduleList[i];
                    break;
                }
            }

            if (schedule.Id == 0)
            {
                var recScheduleList = scheduleList.Where(r => r.Recurring == true).ToList();
                for (int i = 0; i < recScheduleList.Count; i++)
                {
                    var recDate = recScheduleList[i].StartDate;

                    while (recDate <= recScheduleList[i].EndDate)
                    {
                        if (recDate == date)
                        {
                            schedule = recScheduleList[i];
                            break;
                        }

                        recDate.AddDays(7);
                    }

                }

            }


            if (schedule.Id != 0)
            {
                TimeSpan sTime = new TimeSpan();
                sTime = schedule.StartTime;
                
                while (sTime < schedule.EndTime)
                {
                    Appointment slot = new Appointment();
                    slot.DoctorId = doctorId;
                    slot.ClinicId = clinicId;
                    slot.Date = date;
                    slot.Status = 0;
                    slot.StartTime = sTime;
                    slot.EndTime = sTime.Add(new TimeSpan(0, schedule.SlotLength, 0));
                    if (slot.EndTime > schedule.EndTime)
                        break;
                    slotList.Add(slot);
                    sTime = slot.EndTime;
                    
                }

            }

            var bookedList = _context.Appointments.Where(i => i.DoctorId == doctorId && i.ClinicId == clinicId && i.Date == date && i.Status!=0).ToList();

            foreach(var a in slotList)
            {
                foreach(var b in bookedList)
                {
                    if(a.StartTime==b.StartTime && a.EndTime==b.EndTime)
                    {
                        a.Status = b.Status;
                    }
                }
            }
           

            var query = (from slot in slotList
                         select new { slot.DoctorId,slot.ClinicId,slot.Date,slot.Status,startTime=slot.StartTime.ToString(),endTime=slot.EndTime.ToString()}).ToList();

            List<Appointment> apps = new List<Appointment>();
            res.Result = query;

            if (slotList.Count != 0)
            {
                res.Status = true;
                res.Count = slotList.Count;
                res.Message = "All Slots";
                return Ok(res);
            }
            else
            {
                res.Status = false;
                res.Message = "Slot List is Empty";
                return Ok(res);
            }

        }

    }

}





