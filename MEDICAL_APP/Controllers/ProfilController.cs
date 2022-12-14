using MEDICAL_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MEDICAL_APP.Controllers
{
    public class ProfilController : Controller
    {
        // GET: Profil
        [Authorize]
        public ActionResult Index()
        {
          
            var prescriptions = new List<Prescriptions>();
            var medicals = new List<Medical>();
            using (Database1Entities dc = new Database1Entities())
            {
               
                var userIdQuery = dc.Users.Where(x => x.EmailId == this.User.Identity.Name).Select(x => x.UserId);
                int userId = 0;
                foreach (var i in userIdQuery)
                {
                    userId = i;
                }
                
                var medcialQuery = dc.Prescriptions.Where(x => x.UserId == userId).ToList();
                
                foreach (var prescription in medcialQuery)
                {
                    
                    var intakeTimeQuery = dc.IntakeTime.Where(x => x.PrescriptionsId == prescription.PrescriptionsId).ToList();
                    var medical = new Medical();
                    medical.Monday = new List<CheckBoxListItem>();
                    medical.Tuesday = new List<CheckBoxListItem>();
                    medical.Wednesday = new List<CheckBoxListItem>();
                    medical.Thuersday = new List<CheckBoxListItem>();
                    medical.Saturday = new List<CheckBoxListItem>();
                    medical.Sunday = new List<CheckBoxListItem>();

                    medical.MedicalName = prescription.MedicalName;
                    foreach (var intaketime in intakeTimeQuery)
                    {
                        
                      if (intaketime.Day==1)
                        {
                            medical.Monday.Add(new CheckBoxListItem { Hour = intaketime.Hour.ToString() });
                        }
                        if (intaketime.Day == 2)
                        {
                            medical.Tuesday.Add(new CheckBoxListItem { Hour = intaketime.Hour.ToString() });
                        }
                        if (intaketime.Day == 3)
                        {
                            medical.Wednesday.Add(new CheckBoxListItem { Hour = intaketime.Hour.ToString() });
                        }
                        if (intaketime.Day == 4)
                        {
                            medical.Thuersday.Add(new CheckBoxListItem { Hour = intaketime.Hour.ToString() });
                        }
                        if (intaketime.Day == 5)
                        {
                            medical.Friday.Add(new CheckBoxListItem { Hour = intaketime.Hour.ToString() });
                        }
                        if (intaketime.Day == 6)
                        {
                            medical.Saturday.Add(new CheckBoxListItem { Hour = intaketime.Hour.ToString() });
                        }
                        if (intaketime.Day == 7)
                        {
                            medical.Sunday.Add(new CheckBoxListItem { Hour = intaketime.Hour.ToString() });
                        }
                      
                    }
                    medicals.Add(medical);
                }
                
             
            }

            return View("View",medicals);
        }





        // POST: Profil/Delete/5
        [HttpPost]
        public ActionResult Report(string reportName)
        {
            //check for reportName parameter value now
            //to do : Return something
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Delete(string medicalName)
        {
            try
            {
                using (Database1Entities dc = new Database1Entities())
                {
                    var userIdQuery = dc.Users.Where(x => x.EmailId == this.User.Identity.Name).Select(x => x.UserId);
                    int userId = 0;
                    foreach (var i in userIdQuery)
                    {
                        userId = i;
                    }

                    var medcialQuery = dc.Prescriptions.Where(x => x.UserId == userId).ToList();
                    foreach(var prescription in medcialQuery)
                    {
                        if(prescription.MedicalName==medicalName)
                        {
                            dc.Prescriptions.Remove(prescription);
                            dc.SaveChanges();
                            break;
                        }
                    }
                    
                }

                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
