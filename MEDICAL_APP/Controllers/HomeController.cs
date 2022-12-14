using MEDICAL_APP.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MEDICAL_APP.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
           
            return View(CheckBoxesModel());
        }

        [HttpPost]
        public ActionResult Add (Medical medical)
        {
            var isExist = IsMedicalExist(medical.MedicalName);

            if (isExist)
            {
                ModelState.AddModelError("MedicalExist", "Is already exist in base");
                return View("Index",CheckBoxesModel());
            }
        
            using (Database1Entities dc = new Database1Entities())
            {
                var userIdQuery = dc.Users.Where(x => x.EmailId == this.User.Identity.Name).Select(x => x.UserId);
                int userId = 0;
                foreach (var i in userIdQuery)
                {
                    userId = i;
                }
                var z = dc.Prescriptions.Add(new Prescriptions{ MedicalName = medical.MedicalName,UserId=userId});
                
                
 
                foreach (var item in medical.Monday)
                {
                    if (item.IsSelected)
                    {
                       
                        dc.IntakeTime.Add(new IntakeTime { Day = 1, Hour = Int32.Parse(item.Hour)});
                        
                      
                    }
                }
                foreach (var item in medical.Tuesday)
                {
                    if (item.IsSelected)
                    {
                        dc.IntakeTime.Add(new IntakeTime { Day = 2, Hour = Int32.Parse(item.Hour) });
                       

                    }
                }
                foreach (var item in medical.Wednesday)
                {
                    if (item.IsSelected)
                    {
                        dc.IntakeTime.Add(new IntakeTime { Day = 3, Hour = Int32.Parse(item.Hour) });
                    }
                }
                foreach (var item in medical.Thuersday)
                {
                    if (item.IsSelected)
                    {
                        dc.IntakeTime.Add(new IntakeTime { Day = 4, Hour = Int32.Parse(item.Hour) });
                    }
                }
                foreach (var item in medical.Friday)
                {
                    if (item.IsSelected)
                    {
                        dc.IntakeTime.Add(new IntakeTime { Day = 5, Hour = Int32.Parse(item.Hour) });
                    }
                }
                foreach (var item in medical.Saturday)
                {
                    if (item.IsSelected)
                    {
                        dc.IntakeTime.Add(new IntakeTime { Day = 6, Hour = Int32.Parse(item.Hour) });
                    }
                }
                foreach (var item in medical.Sunday)
                {
                    if (item.IsSelected)
                    {
                        dc.IntakeTime.Add(new IntakeTime { Day = 7, Hour = Int32.Parse(item.Hour) });
                    }
                }
                dc.SaveChanges();
                CheckBoxesModel();
                return View("Index",CheckBoxesModel());

            }
        }
        [NonAction]
        public bool IsMedicalExist(string MedicalName)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var userIdQuery = dc.Users.Where(x => x.EmailId == this.User.Identity.Name).Select(x => x.UserId);
                int userId = 0;
                foreach (var i in userIdQuery)
                {
                    userId = i;
                }

                var prescriptions = dc.Prescriptions.Where(a => a.UserId == userId).ToList();
                foreach (var prescription  in prescriptions)
                {
                    if (prescription.MedicalName==MedicalName)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        [NonAction]
        public int GenerateId()
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Medicals.Max(x => x.MedicalId);

                return v + 1;
            }
        }
        [NonAction]
        public Medical CheckBoxesModel()
        {
            var model = new Medical();
            model.Monday = new List<CheckBoxListItem>();
            for (int i = 1; i < 25; i++)
            {
                model.Monday.Add(new CheckBoxListItem() { IsSelected = false, Hour = i.ToString() });

            };
            model.Tuesday = new List<CheckBoxListItem>();
            for (int i = 1; i < 25; i++)
            {
                model.Tuesday.Add(new CheckBoxListItem() { IsSelected = false, Hour = i.ToString() });

            };
            model.Wednesday = new List<CheckBoxListItem>();
            for (int i = 1; i < 25; i++)
            {
                model.Wednesday.Add(new CheckBoxListItem() { IsSelected = false, Hour = i.ToString() });

            };
            model.Thuersday = new List<CheckBoxListItem>();
            for (int i = 1; i < 25; i++)
            {
                model.Thuersday.Add(new CheckBoxListItem() { IsSelected = false, Hour = i.ToString() });

            };
            model.Friday = new List<CheckBoxListItem>();
            for (int i = 1; i < 25; i++)
            {
                model.Friday.Add(new CheckBoxListItem() { IsSelected = false, Hour = i.ToString() });

            };
            model.Saturday = new List<CheckBoxListItem>();
            for (int i = 1; i < 25; i++)
            {
                model.Saturday.Add(new CheckBoxListItem() { IsSelected = false, Hour = i.ToString() });

            };
            model.Sunday = new List<CheckBoxListItem>();
            for (int i = 1; i < 25; i++)
            {
                model.Sunday.Add(new CheckBoxListItem() { IsSelected = false, Hour = i.ToString() });

            };
            return model;
        }
    }
}