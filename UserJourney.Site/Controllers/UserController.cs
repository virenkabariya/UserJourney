using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserJourney.Repositories.EF.ViewModel;
using UserJourney.Core.Constants;
using UserJourney.Core.Enums;
using UserJourney.Core.Services;
using UserJourney.Repositories.Contracts;
using UserJourney.Repositories.EF;
using UserJourney.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace UserJourney.Site.Controllers
{
    public class UserController : Controller
    {
        private IProjectUnitOfWork _unitOfWork { get; set; }
        private IHttpContextAccessor _contextAccessor;
        private IGenericRepository<Users> _userRepository;
        private IHelperService _helperService;
        private IEmailService _emailService;
        private IClaimsManager _claimsManager;

        #region Constructor
        public UserController(IProjectUnitOfWork unitOfWork, IHelperService helperService, IEmailService emailService, IHttpContextAccessor contextAccessor, IClaimsManager claimsManager)
        {
            _unitOfWork = unitOfWork;
            _helperService = helperService;
            _emailService = emailService;
            _contextAccessor = contextAccessor;
            _userRepository = _unitOfWork.GetRepository<Users>();
            _claimsManager = claimsManager;
        }
        #endregion

        [HttpGet]
        [ActionName(Actions.Login)]
        public async Task<ActionResult> Login()
        {
            LoginViewModel model = new LoginViewModel();

            if (Request.Cookies[EnumHelper.GetDescription(CookieName.IsRemember)] != null && Request.Cookies[EnumHelper.GetDescription(CookieName.UserName)] != null && Request.Cookies[EnumHelper.GetDescription(CookieName.Password)] != null)
            {
                model.RememberMe = ConvertTo.ToBoolean(EncryptionDecryption.GetDecrypt(Request.Cookies[EnumHelper.GetDescription(CookieName.IsRemember)]));
                if (model.RememberMe)
                {
                    if (Request.Cookies[EnumHelper.GetDescription(CookieName.UserName)] != null)
                    {
                        model.Email = EncryptionDecryption.GetDecrypt(Request.Cookies[EnumHelper.GetDescription(CookieName.UserName)]);
                    }

                    if (Request.Cookies[EnumHelper.GetDescription(CookieName.Password)] != null)
                    {
                        model.Password = EncryptionDecryption.GetDecrypt(Request.Cookies[EnumHelper.GetDescription(CookieName.Password)]);
                    }
                }
            }

            return View(Views.Login, model);
        }

        [HttpPost]
        [ActionName(Actions.Login)]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetFirstAsync(s => s.Email == model.Email);

                if (user != null && user.UserId > 0)
                {
                    if (user.Password == EncryptionDecryption.GetEncrypt(model.Password))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        this.TempData["ErrorMessage"] = SystemMessage.InvalidUserNameandPassword;
                    }

                    if (isSuccess)
                    {
                        if (model.RememberMe)
                        {
                            _helperService.AddCookie(EnumHelper.GetDescription(CookieName.UserName), model.Email);
                            _helperService.AddCookie(EnumHelper.GetDescription(CookieName.Password), model.Password);
                            _helperService.AddCookie(EnumHelper.GetDescription(CookieName.IsRemember), Convert.ToString(model.RememberMe));
                        }
                        else
                        {
                            _helperService.RemoveCookie(EnumHelper.GetDescription(CookieName.UserName));
                            _helperService.RemoveCookie(EnumHelper.GetDescription(CookieName.Password));
                            _helperService.RemoveCookie(EnumHelper.GetDescription(CookieName.IsRemember));
                        }

                        var claims = new List<Claim>();
                        claims.Add(new Claim(CustomClaimTypes.UserId.ToString(), user.UserId.ToString()));
                        claims.Add(new Claim(CustomClaimTypes.FirstName.ToString(), user.FirstName.ToString()));
                        claims.Add(new Claim(CustomClaimTypes.LastName.ToString(), user.LastName.ToString()));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                       
                        await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties());
                        return RedirectToAction(Actions.Dashboard, UserJourney.Core.Constants.Controllers.HomeController);
                    }
                }
                else
                {
                    this.TempData["ErrorMessage"] = SystemMessage.InvalidUserNameandPassword;
                }
            }

            return View(Views.Login, model);
        }

        [HttpGet]
        [ActionName(Actions.Registration)]
        public async Task<ActionResult> Registration()
        {
            UserViewModel model = new UserViewModel();
            return View(Views.Registration, model);
        }

        [HttpPost]
        [ActionName(Actions.Registration)]
        public async Task<IActionResult> Registration(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetFirstAsync(s => s.Email == model.Email);

                if (user != null && user.UserId > 0)
                {
                    this.TempData["ErrorMessage"] = SystemMessage.EmailDuplicateMessage;
                }
                else
                {
                    user = new Users();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.Password = EncryptionDecryption.GetEncrypt(model.Password);
                    user.PhoneNumber = model.PhoneNumber;
                    user.CreatedDate = DateTime.Now;
                    await _userRepository.CreateAsync(user);
                    await _unitOfWork.SaveAsync();
                    this.TempData["SuccessMessage"] = SystemMessage.UserRegistrationMessage;
                    return RedirectToAction(Actions.Login, UserJourney.Core.Constants.Controllers.UserController);
                }
            }

            return View(Views.Registration, model);
        }

        [HttpGet]
        [ActionName(Actions.ForgotPassword)]
        public async Task<ActionResult> ForgotPassword()
        {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();
            return View(Views.ForgotPassword, model);
        }


        [HttpPost]
        [ActionName(Actions.ForgotPassword)]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                var user = await _userRepository.GetFirstAsync(s => s.Email == model.Email);

                if (user != null && user.UserId > 0)
                {
                    Guid token = Guid.NewGuid();
                    var keyValues = new Dictionary<string, string>()
                            {
                                { "[@Link]", string.Format(@"{0}://{1}{2}/{3}/{4}?encid={5}&token={6}", _contextAccessor.HttpContext.Request.Scheme, _contextAccessor.HttpContext.Request.Host.ToString(), _contextAccessor.HttpContext.Request.PathBase.ToString(), UserJourney.Core.Constants.Controllers.UserController, Actions.ResetForgotPassword, EncryptionDecryption.GetEncrypt(user.UserId.ToString()), EncryptionDecryption.GetEncrypt(token.ToString()))}
                            };

                    string emailTemplateFileName = string.Format("{0}.html", EmailTemplateFileName.ResetPasswordEmailTemplate.ToString());
                    var success = await _emailService.SendEmailAsync(new List<string>() { user.Email }, new List<string>(), "Reset Password", emailTemplateFileName, keyValues);

                    if (success)
                    {
                        user.LastTokenCreatedDate = DateTime.Now;
                        user.PasswordResetToken = token.ToString();
                        await _userRepository.UpdateAsync(user);
                        await _unitOfWork.SaveAsync();
                        this.TempData["SuccessMessage"] = SystemMessage.LinkSent;
                    }
                    else
                    {
                        this.TempData["ErrorMessage"] = SystemMessage.ErrorOnSendingMail;
                    }
                }
                else
                {
                    this.TempData["ErrorMessage"] = SystemMessage.EmailValidMessage;
                }
            }
            catch (Exception ex)
            {
                this.TempData["ErrorMessage"] = SystemMessage.ModelIsNotValid;
            }

            return this.View(Views.ForgotPassword, model);
        }

        [HttpGet]
        [ActionName(Actions.ResetForgotPassword)]
        public async Task<ActionResult> ResetForgotPassword(string encid, string token)
        {
            int decryptedID = EncryptionDecryption.GetDecrypt(encid).ToInteger();
            var user = await _userRepository.GetByIdAsync(decryptedID);

            if (user != null && user.UserId > 0)
            {
                if (user.PasswordResetToken != null
                    && user.PasswordResetToken == EncryptionDecryption.GetDecrypt(token)
                    && user.LastTokenCreatedDate != null
                    && user.LastTokenCreatedDate.ToDate().AddMinutes(30) > DateTime.Now)
                {
                    ViewBag.HasError = false;
                }
                else
                {
                    ViewBag.HasError = true;
                }
            }
            else
            {
                ViewBag.HasError = true;
            }

            ResetForgotPasswordViewModel model = new ResetForgotPasswordViewModel();
            model.UserId = decryptedID;

            return this.View(Views.ResetForgotPassword, model);
        }

        [HttpPost]
        [ActionName(Actions.ResetForgotPassword)]
        public async Task<ActionResult> ResetForgotPassword(ResetForgotPasswordViewModel model)
        {
            ViewBag.HasError = false;

            try
            {
                var user = await _userRepository.GetByIdAsync(model.UserId);
                user.Password = EncryptionDecryption.GetEncrypt(model.Password);
                user.PasswordResetToken = null;
                user.LastTokenCreatedDate = null;

                user = await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveAsync();

                if (user.Password == EncryptionDecryption.GetEncrypt(model.Password))
                {
                    this.TempData["SuccessMessage"] = SystemMessage.PasswordUpdated;
                    return this.RedirectToAction(Actions.Login, UserJourney.Core.Constants.Controllers.UserController);
                }
            }
            catch (Exception ex)
            {
                this.TempData["ErrorMessage"] = SystemMessage.ModelIsNotValid;
            }

            return this.View(Views.ResetForgotPassword, model);
        }

        [HttpGet]
        [ActionName(Actions.ResetPassword)]
        [Authorize]
        public async Task<ActionResult> ResetPassword()
        {
            var user = await _userRepository.GetByIdAsync(_claimsManager.GetCurrentUserId());
            ViewBag.HasError = false;

            if (user == null || user.UserId < 0)
            {
                ViewBag.HasError = true;
            }

            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.UserId = _claimsManager.GetCurrentUserId();

            return this.View(Views.ResetPassword, model);
        }

        
        [HttpPost]
        [ActionName(Actions.ResetPassword)]
        [Authorize]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                ViewBag.HasError = false;
                var user = await _userRepository.GetByIdAsync(model.UserId);

                if (EncryptionDecryption.GetEncrypt(model.OldPassword) == user.Password)
                {
                    if (EncryptionDecryption.GetEncrypt(model.Password) == user.Password)
                    {
                        this.TempData["ErrorMessage"] = SystemMessage.OldNewPasswordsSame;
                    }
                    else
                    {
                        user.Password = EncryptionDecryption.GetEncrypt(model.Password);

                        await _userRepository.UpdateAsync(user);
                        await _unitOfWork.SaveAsync();

                        this.TempData["SuccessMessage"] = SystemMessage.PasswordUpdated;
                        return this.RedirectToAction(Actions.Dashboard, UserJourney.Core.Constants.Controllers.HomeController);
                    }
                }
                else
                {
                    this.TempData["ErrorMessage"] = SystemMessage.IncorrectOldPassword;
                }
            }
            catch (Exception ex)
            {
                this.TempData["ErrorMessage"] = SystemMessage.ModelIsNotValid;
            }

            return this.View(Views.ResetPassword, model);
        }

        [HttpGet]
        [ActionName(Actions.Logout)]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(Actions.Login);
        }

    }
}
