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
    public class RequestsController : BaseController
    {

        private readonly wHealthappDbContext _context;
        public RequestsController(wHealthappDbContext context)
        {
            _context = context;
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
            catch(Exception ex)
            {
                throw;
            }

            response.Message = "Your request has been SUBMITTED.And you will be informed if clinic APPROVED your request";
            return Ok(response);
        }


    }
}




