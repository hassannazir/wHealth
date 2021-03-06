﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using wHealthApi.Helpers;
using wHealthApi.Models;
using wHealthApi.Models.JoinModels;

namespace wHealthApi.Controllers
{

    public class UsersController : BaseController
    {
        private readonly wHealthappDbContext _context;
        public UsersController(wHealthappDbContext context)
        {
            _context = context;
        }
        
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Create(AppUser appUser)
        {
            try
            {
                Patient patient = new Patient();
                Doctor doctor = new Doctor();
                Clinic clinic = new Clinic();
                User user = new User();           
                Response response = new Response();
                user.Status = "InActive";



                if (appUser.Type == AppConstants.Patient)
                {
                    var email = _context.Patients.Where(u => u.Email == appUser.Email).FirstOrDefault();
                    var usrname = _context.Users.Where(u => u.Username == appUser.Username).FirstOrDefault();


                    if (email != null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! This Email Already Exist";
                        return Ok(response);
                    }

                    if (usrname != null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! This username Already Exist";
                        return Ok(response);
                    }

                    patient.Name = appUser.Name;
                    patient.Email = appUser.Email;
                    patient.PhoneNo = appUser.PhoneNo;
                    patient.Address = appUser.Address;

       
                    try
                    {
                        await _context.Patients.AddAsync(patient);
                        await _context.SaveChangesAsync();
;
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }


                    user.PatientId = patient.Id;


                }

                else if (appUser.Type == AppConstants.Doctor)
                {
                    var email = _context.Doctors.Where(e => e.Email == appUser.Email).FirstOrDefault();
                    var usrname = _context.Users.Where(u => u.Username == appUser.Username).FirstOrDefault();
                    if (email != null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! This Email Already Exist";
                        return Ok(response);
                    }

                    if (usrname != null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! This username Already Exist";
                        return Ok(response);
                    }

                    doctor.Name = appUser.Name;
                    doctor.Email = appUser.Email;
                    doctor.PhoneNo = appUser.PhoneNo;
                    doctor.Address = appUser.Address;
                    doctor.LicenseNo = appUser.LicenseNo;
                    doctor.Experience = appUser.Experience;
                    doctor.Qualification = appUser.Qualification;
                    try
                    {
                        await _context.Doctors.AddAsync(doctor);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }


                    user.DoctorId = doctor.Id;

                }

                else if (appUser.Type == AppConstants.Clinic)
                {
                    var email = _context.Clinics.Where(e => e.Email == appUser.Email).FirstOrDefault();
                    var usrname = _context.Users.Where(u => u.Username == appUser.Username).FirstOrDefault();
                    if (email != null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! This Email Already Exist";
                        return Ok(response);
                    }

                    if (usrname != null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! This username Already Exist";
                        return Ok(response);
                    }

                    var reg = _context.Clinics.Where(r => r.RegistrationNo == appUser.RegistrationNo).FirstOrDefault();
                    if (reg != null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! This Registeration Number Already Exist";
                        return Ok(response);
                    }


                    clinic.Name = appUser.Name;
                    clinic.Email = appUser.Email;
                    clinic.PhoneNo = appUser.PhoneNo;
                    clinic.Address = appUser.Address;
                    clinic.RegistrationNo = appUser.RegistrationNo;

                    try
                    {
                        await _context.Clinics.AddAsync(clinic);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }


                    user.ClinicId = clinic.Id;
                    
                  
                }

                user.Username = appUser.Username;
                user.Password = appUser.Password;
                user.RegisteredDate = DateTime.Now;
                user.Type = appUser.Type;

                try
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }

                try
                {
                    if (appUser.Type != AppConstants.Clinic)
                    {
                        MailMessage em = new MailMessage();
                        em.To.Add(appUser.Email);
                        em.From = new MailAddress("dev@theta.solutions", "wHealth");
                        em.Subject = "EMAIL VERFICATION FOR wHEALTH APP.";

                        // em.Body = "<h4>Dear " + appUser.Name + ", </h4><br>Please click on the following link to confirm your email.<br><br>" + "<a href='https://localhost:44340/Id?Id=" + user.Username + "'>CLICK HERE! </a><br><br>  Thanks.";
                        em.Body = "<h4>Dear " + appUser.Name + ", </h4><br>Please click on the following link to confirm your email.<br><br>" + "<a href='http://whealthapi.azurewebsites.net/Id?Id=" + user.Username + "'>CLICK HERE! </a><br><br>  Thanks.";

                        em.IsBodyHtml = true;

                        SmtpClient cli = new SmtpClient();
                        cli.Host = "116.202.175.92";
                        cli.Port = 25;
                        cli.UseDefaultCredentials = false;
                        cli.Credentials = new NetworkCredential("dev@theta.solutions", "8xTc$NU?%5kCh.5L&,u#:o^S|oCp");
                        cli.EnableSsl = false;

                        await cli.SendMailAsync(em);
                    }
                }
                catch (Exception ex )
                {

                    throw;
                }
                if (appUser.Type != AppConstants.Clinic)
                {
                    response.Status = true;
                    response.Result = "Registeration Completed. Please Verify Your Email First to Use the APP.";
                    return Ok(response);
                }
                else
                {
                    response.Status = true;
                    response.Result = "You will be informed when admin approves your request. Untill then, Please wait.";
                    return Ok(response);
                }
            }
            catch
            {
                return Ok();
            }
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> Edit(int id, AppUser appUser)
        {
            try
            {
                Response response = new Response();
                response.Status = true;
                response.Result = "Successfully Updated.";

                if (appUser.Type == AppConstants.Patient)
                {
                    var pat = await _context.Patients.FindAsync(id);
                    if (pat == null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! User does not exist";
                        return Ok(response);
                    }

                    pat.Address = appUser.Address;
                    pat.Name = appUser.Name;
                    pat.PhoneNo = appUser.PhoneNo;

                    _context.Patients.Update(pat);
                    _context.SaveChanges();
                }
                else if (appUser.Type == AppConstants.Doctor)
                {
                    var doc = await _context.Doctors.FindAsync(id);
                    if (doc == null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !!! User does not exist";
                        return Ok(response);
                    }

                    doc.Address = appUser.Address;
                    doc.Name = appUser.Name;
                    doc.PhoneNo = appUser.PhoneNo;
                    doc.Experience = appUser.Experience;
                    doc.Qualification = appUser.Qualification;

                    _context.Doctors.Update(doc);
                    _context.SaveChanges();
                }
                else if (appUser.Type == AppConstants.Clinic)
                {
                    var clinic = await _context.Clinics.FindAsync(id);
                    if (clinic == null)
                    {
                        response.Status = false;
                        response.Result = "Sorry !! User does not exist";
                        return Ok(response);
                    }

                    clinic.Address = appUser.Address;
                    clinic.Name = appUser.Name;
                    clinic.PhoneNo = appUser.PhoneNo;

                    _context.Clinics.Update(clinic);
                    _context.SaveChanges();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

       

    }
}
