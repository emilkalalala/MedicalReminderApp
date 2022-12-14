using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MEDICAL_APP.Models;

namespace MEDICAL_APP.Controllers
{
    public class UserController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] Users users)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                #region//Email is already exist
                var isExist = IsEmailExist(users.EmailId);

                if (isExist)
                {
                    ModelState.AddModelError("EmailExist","Is already exist in base");
                    return View(users);
                }
                #endregion

                #region Generate Activation Code

                users.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing 
                users.Password = Crypto.Hash(users.Password);
                users.ConfirmPassword = Crypto.Hash(users.ConfirmPassword);
                #endregion

                users.IsEmailVerified = false;

                #region Save To Database
                using (Database1Entities dc = new Database1Entities())
                {
                    dc.Users.Add(users);
                    dc.SaveChanges();

                    SendVerificationLinkEmail(users.EmailId, users.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + users.EmailId;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(users);


        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (Database1Entities dc = new Database1Entities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Verify Email

        //Verify Email Link
        //Login

        //Login POST

        //Login out
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login , string ReturnUrl = "")
        {
            string message = "";
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Users.Where(a => a.EmailId == login.EmailId).FirstOrDefault();
                if (v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailId, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using(Database1Entities dc = new Database1Entities())
            {
                var v = dc.Users.Where(a => a.EmailId == emailID).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("medicalapptswm@gmail.com", "Medical App");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "TswmXD11"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

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