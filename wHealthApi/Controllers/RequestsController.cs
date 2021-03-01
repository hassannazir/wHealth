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


        

    }
}




