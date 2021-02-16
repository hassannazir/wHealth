using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using wHealthApi.Helpers;
using wHealthApi.Models;
using wHealthApi.Models.JoinModels;

namespace wHealthApi.Controllers
{

    public class AccountsController : BaseController
    {
        private readonly wHealthappDbContext _context;
        private IConfiguration Config { get; }

        public AccountsController(IConfiguration config, wHealthappDbContext context)
        {
            Config = config;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string username, string pass)
        {
            Response httpResponse = new Response();
            User login = new User();
            login.Username = username;
            login.Password = pass;
            IActionResult result = Unauthorized();
            AuthenticateUser(login);
            var user = AuthenticateUser(login);

            if (user != null)
            {

                if (user.Status != "Active")
                {
                    httpResponse.Status = false;
                    httpResponse.Message = "YOU NEED TO VERIFY YOUR EMAIL FIRST.";
                    return Ok(httpResponse);
                }
                
                //ELSE A TOKEN WILL BE GENERATED AND MOVE ON TO SUCCESSFULL LOGIN
                
                var tokenStr = GenerateJSONWebToken(user);
                httpResponse.Status = true;
                httpResponse.Token = tokenStr;
                if (user.Type== AppConstants.Patient)
                {
                    AppUser au = new AppUser(); 
                    var specificUserTableData = _context.Patients.Where(u => u.Id == user.PatientId).FirstOrDefault();
                    var usersTableData = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

                    au.Id = specificUserTableData.Id;
                    au.Username = usersTableData.Username;
                    au.Password = usersTableData.Password;
                    au.Name = specificUserTableData.Name;
                    au.Email = specificUserTableData.Email;
                    au.PhoneNo = specificUserTableData.PhoneNo;
                    au.Address = specificUserTableData.Address;

                    var U = au;

                    httpResponse.Result=U;
                    return Ok(httpResponse);
                }
                else if(user.Type == AppConstants.Doctor)
                {

                    AppUser doc = new AppUser();
                    var specificUserTableData = _context.Doctors.Where(u => u.Id == user.PatientId).FirstOrDefault();
                    var usersTableData = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

                    doc.Id = specificUserTableData.Id;
                    doc.Username = usersTableData.Username;
                    doc.Password = usersTableData.Password;
                    doc.Name = specificUserTableData.Name;
                    doc.Email = specificUserTableData.Email;
                    doc.PhoneNo = specificUserTableData.PhoneNo;
                    doc.Address = specificUserTableData.Address;
                    doc.LicenseNo = specificUserTableData.LicenseNo;
                    doc.Qualification = specificUserTableData.Qualification;
                    doc.Experience = specificUserTableData.Experience;


                    var U = doc;
                    httpResponse.Result = U;
                    return Ok(httpResponse);
                
                
                }
                else if(user.Type == AppConstants.Clinic)
                {
                    AppUser cli = new AppUser();
                    var specificUserTableData = _context.Clinics.Where(u => u.Id == user.PatientId).FirstOrDefault();
                    var usersTableData = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

                    cli.Id = specificUserTableData.Id;
                    cli.Username = usersTableData.Username;
                    cli.Password = usersTableData.Password;
                    cli.Name = specificUserTableData.Name;
                    cli.Email = specificUserTableData.Email;
                    cli.PhoneNo = specificUserTableData.PhoneNo;
                    cli.Address = specificUserTableData.Address;
                    cli.RegistrationNo = specificUserTableData.RegistrationNo;

               


                    var U = cli;
                    httpResponse.Result = U;
                    return Ok(httpResponse);

                }

            }

            httpResponse.Status = false;
            httpResponse.Message = "INVALID CREDENTIALS";
            return Ok(httpResponse);
        }

        private User AuthenticateUser(User login)
        {

            User user = null;
            user = _context.Users.Where(u => u.Username == login.Username && u.Password == login.Password).FirstOrDefault();
            return user;
        }

        private string GenerateJSONWebToken(User userinfo)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub,userinfo.Username),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: Config["Jwt:Issuer"],
                audience: Config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;

        }

      

    }
}
