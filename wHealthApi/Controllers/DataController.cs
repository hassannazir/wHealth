using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wHealthApi.Models;
using wHealthApi.Models.JoinModels;

namespace wHealthApi.Controllers
{
    public class DataController : BaseController
    {

        private readonly wHealthappDbContext _context;
        public DataController(wHealthappDbContext context)
        {
            _context = context;
        }



        //RETURNING ALL THE REQUIRED CLINICS DATA
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(int doc_id)
        {
            try
            {

                Response res = new Response();

              IList <Clinic> ClinicList = _context.Clinics.ToList();
                IList<User> usersList = _context.Users.ToList();
                IList<Doctorclinic> docClincicList = _context.Doctorclinics.ToList();

                var result = (from c in ClinicList
                             join u in usersList on c.Id equals u.ClinicId                             
                             where  !docClincicList.Any(dc=>dc.ClinicId==c.Id)
                             select new { c.Email, c.Name, c.PhoneNo, c.RegistrationNo, c.Id, c.Address }).ToList();


                res.Status = true;
                res.Result = result;
                res.Message = "FOLLOWING CLINICS AVAILABLE";
                return Ok(res);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


    


        //RETURNING A SPECIFIC USER DATA
        [HttpPut]
        [AllowAnonymous]
        public IActionResult GetSpecific(int id)
        {
            try
            {
                Response res = new Response();

                var DoctorData = _context.Doctors.Find(id);
                if (DoctorData != null)
                {
                    res.Message = "Selected Doctor's data.";
                    res.Result = DoctorData;
                    res.Status = true;
                    return Ok(res);
                }

                var PatientData = _context.Patients.Find(id);
                if (PatientData != null)
                {
                    res.Message = "Selected Patient's data.";
                    res.Result = PatientData;
                    res.Status = true;
                    return Ok(res);
                }

                var ClinicData = _context.Clinics.Find(id);
                if (ClinicData != null)
                {
                    res.Message = "Selected Clinic's data.";
                    res.Result = ClinicData;
                    res.Status = true;
                    return Ok(res);
                }

                res.Message = "id not found";
                res.Result = null;
                res.Status = false;
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //RETURNING ALL DOCTORS DATA IN CLINIC ID
        [HttpPatch]
        [AllowAnonymous]
        public IActionResult DocsInClinic(int id)
        {
            try
            {
                Response res = new Response();
                IList<Clinic> ClinicList = _context.Clinics.ToList();
                var query = (from c in _context.Doctorclinics
                             join u in _context.Doctors
                             on c.DoctorId equals u.Id
                             where c.ClinicId == id
                             select new { u.Id, u.Name, u.PhoneNo, u.Email, u.Address, u.Experience, u.LicenseNo, u.Qualification }).ToList();  
                    res.Status = true;
                    res.Result = query;
                    res.Message = "FOLLOWING DOCTORS INCLUDED";
                    return Ok(res);
            }
            catch (Exception ex)
            {

                throw;
            }

           
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ActiveDoctors(int Cid)
        {
            try
            {
                Response res = new Response();

                var query = (from c in _context.Doctors
                             join u in _context.Doctorclinics
                             on c.Id equals u.DoctorId
                             where u.ClinicId == Cid
                             where u.Status == "Active"
                             select new { c.Id, c.Name, c.Email, c.PhoneNo, c.Address, c.ProfilePic, c.LicenseNo, c.Qualification, c.Experience }).ToList();

                if (!query.Any())
                {
                    res.Status = false;

                    res.Result = query;
                    res.Message = "No active Doctor";
                    return Ok(res);
                }

                res.Status = true;
                res.Result = query;
                res.Message = "All active doctors in this Clinic";
                return Ok(res);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete]
        [AllowAnonymous]
        public IActionResult InActiveDoctors(int Cid)
        {
            try
            {
                Response res = new Response();

                var query = (from c in _context.Doctors
                             join u in _context.Doctorclinics
                             on c.Id equals u.DoctorId
                             where u.ClinicId == Cid
                             where u.Status == "InActive"
                             select new { c.Id, c.Name, c.Email, c.PhoneNo, c.Address, c.ProfilePic, c.LicenseNo, c.Qualification, c.Experience }).ToList();

                if (!query.Any())
                {
                    res.Status = false;

                    res.Result = query;
                    res.Message = "No InActive Doctor";
                    return Ok(res);
                }

                res.Status = true;
                res.Result = query;
                res.Message = "All InActive doctors in this Clinic";
                return Ok(res);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    
       
    }
}




    




