using MEDICAL_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MEDICAL_APP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        int lastHour;
        protected void Application_Start()
        {
            var aTimer = new System.Timers.Timer(1000); //One second, (use less to add precision, use more to consume less processor time
            lastHour = DateTime.Now.Hour-1;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (lastHour < DateTime.Now.Hour || (lastHour == 23 && DateTime.Now.Hour == 0))
            {
                lastHour = DateTime.Now.Hour;
                CheckingData(); 
            }

        }
        private void CheckingData()
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var day = DateTime.Now.DayOfWeek;
                var usersIdList = dc.Users.Select(x => x.UserId);
                foreach (var userid in usersIdList)
                {
                    var email = dc.Users.Where(x => x.UserId == userid).Select(x => x.EmailId).FirstOrDefault();
                    var prescriptionList = dc.Prescriptions.Where(x => x.UserId == userid);
                    foreach (var prescription in prescriptionList)
                    {
                        var intakeTimeList = dc.IntakeTime.Where(x => x.PrescriptionsId == prescription.PrescriptionsId);
                        foreach (var intaketime in intakeTimeList)
                        {
                            if (((DayOfWeek)intaketime.Day == day || (intaketime.Day == 7 && day == DayOfWeek.Sunday)) && (intaketime.Hour == lastHour || (intaketime.Hour == 24 && lastHour == 0))) 
                            {
                                SendMail(email, prescription.MedicalName);
                                break;
                            }
                        }
                    }

                }

            }
        }

        private void SendMail(string email, string medicalName)
        {


            var fromEmail = new MailAddress("medicalapptswm@gmail.com", "Medical App");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "TswmXD11";
            string subject = "REMEMBER ABOUT MEDICAL";

            string body = "<br/><br/>Its time to take:" +     
                medicalName +
                " <br/><br/>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}

