using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using wHealth;

namespace wHealth.Controllers
{
    public class ClinicsController : Controller
    {
        private readonly whealthappdbContext _context;

        public ClinicsController(whealthappdbContext context)
        {
            _context = context;
        }

        // GET: Clinics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clinics.ToListAsync());
        }

        // GET: Clinics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // GET: Clinics/Create
        public IActionResult Create()
        {
            return View();
        }

        
        // RETURNING CLINICS OBJECT
        public  async Task<IActionResult> Edit(int? id)
        {

            var user = _context.Users.Where(d => d.ClinicId == id).FirstOrDefault();
            //var clin = _context.Clinics.Where(d => d.Id == id).FirstOrDefault();

            
            if (user == null)
            {
                return NotFound();
            }
            
            
            return View(user);
        }

        

        //CHANGE STATUS IN USER OBJECT FOR THE CLINICS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditClinicStatus(int cid, string Status)
        {
           

                try
                {
                //var us = _context.Users.Find(ClinicId);

                var us = _context.Users.Where(i => i.ClinicId == cid).FirstOrDefault();
                    if (us != null)
                    {
                        us.Status = Status;
                        _context.Update(us);
                        _context.SaveChanges();
                        ViewBag.message = "Clinics status has been changed.";
                        return RedirectToAction("Index");

                }

            }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ClinicExists(cid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            ViewBag.message1 = cid;
            return RedirectToAction("Index");
        
        }





        // GET: Clinics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // POST: Clinics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            _context.Clinics.Remove(clinic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClinicExists(int id)
        {
            return _context.Clinics.Any(e => e.Id == id);
        }
    }
}
