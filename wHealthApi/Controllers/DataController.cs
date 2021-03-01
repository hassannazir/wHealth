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



        //RETURNING ALL THE ACTIVE STATUS CLINICS DATA
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                Response res = new Response();
                IList<Clinic> ClinicList = _context.Clinics.ToList();
                var query = (from c in _context.Clinics
                             join u in _context.Users
                             on c.Id equals u.ClinicId
                             where u.Status == "Active"
                             select new { c.Email, c.Name, c.PhoneNo, c.RegistrationNo, c.Id, c.Address }).ToList();

                res.Status = true;
                res.Result = query;
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
        public IActionResult Get(int id)
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
    }
}




    




