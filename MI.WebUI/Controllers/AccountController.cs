using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MI.WebUI.Models;
using MI.Web.Controllers;
using MI.Web.Infrastructure;
using MI.Web.Model;
using MI.Domain.Models;

namespace MI.WebUI.Controllers
{
    public class AccountController : ApplicationController
    {

        //
        // GET: /Account/LogOn
        Users _users;
        // ITokenHandler _tokenStore;

        public AccountController()
            : this(new FormsAuthTokenStore())
        {

        }


        public AccountController(ITokenHandler tokenStore)
            : base(tokenStore)
        {
            _users = new Users();
            // this._tokenStore = tokenStore;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(string email, string password)
        {
            dynamic result = _users.Login(email, password);
            if (result.Authenticated)
            {
                SetToken(result.User.ID);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = result.Message;
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        //private void SetToken(dynamic userID)
        //{
        //    var token = Guid.NewGuid().ToString();
        //    //updae db with token
        //    _users.SetToken(token, userID);
        //    _tokenStore.SetClientAccess(token);
        //}

        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register




        [HttpPost]
        public ActionResult Register(string Email, string Password, string Confirm)
        {

            var result = _users.Register(Email, Password, Confirm);
            if (result.Success)
            {

                //FormsAuthentication.SetAuthCookie(Email, false /* createPersistentCookie */);

                //var token = Guid.NewGuid().ToString();
                //_tokenStore.SetClientAccess(token);
                var myid = result.UserID.ID;
                SetToken(myid);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = result.Message;
            }
            return View();
        }


        private void SetToken(dynamic userID)
        {
            //Set token to GUID if you want one and only one person logged in with given credentials
            //var token = Guid.NewGuid().ToString();

            string crit = "ID =" + userID.ToString();
            var result = _users.Single(where: crit);

            var token = result.Email;

            _users.SetToken(token, userID);
            TokenStore.SetClientAccess(token);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult LogOn(LogOnModel model, string returnUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (Membership.ValidateUser(model.UserName, model.Password))
        //        {
        //            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
        //            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
        //                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
        //            {
        //                return Redirect(returnUrl);
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "The user name or password provided is incorrect.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        //[HttpPost]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Attempt to register the user
        //        MembershipCreateStatus createStatus;
        //        Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(createStatus));
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
