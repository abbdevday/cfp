using System;
using System.IO;
using System.Linq;
using DevDayCFP.Common;
using DevDayCFP.Extensions;
using DevDayCFP.Models;
using DevDayCFP.Services;
using DevDayCFP.ViewModels;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Security;
using Nancy.Validation;

namespace DevDayCFP.Modules
{
    public class AccountModule : BaseModule
    {
        private readonly IEmailService _emailService;

        public AccountModule(IDataStore dataStore, IEmailService emailService, IRootPathProvider pathProvider)
            : base("account", dataStore)
        {
            _emailService = emailService;
            Get["/login"] = parameters =>
            {
                if (Context.CurrentUser != null)
                {
                    return new RedirectResponse("/");
                }

                var loginModel = new LoginViewModel();

                return View["Login", loginModel];
            };

            Post["/login"] = parameters =>
            {
                var model = this.Bind<LoginViewModel>();
                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Login", model];
                }

                var userMapper = new UserMapper(dataStore);
                var userGuid = userMapper.ValidateUser(model.UserName, model.Password);

                if (userGuid == null)
                {
                    ViewBag.Page.Value.Errors.Add(new ErrorViewModel
                    {
                        Name = "UserName",
                        ErrorMessage = "Sorry! Either username or password is wrong!"
                    });
                    return View["Login", model];
                }

                DateTime? expiry = null;
                if (model.RememberMe)
                {
                    expiry = DateTime.Now.AddDays(30);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Post["/inactive"] = parameters =>
            {
                if (Context.CurrentUser == null)
                {
                    return new RedirectResponse("/");
                }

                var user = (User)Context.CurrentUser;

                var mailViewModel = new RegisterMailViewModel();
                mailViewModel.User = user;
                mailViewModel.Hostname = BuildHostname(Context.Request.Url);
                var registrationMailContent = this.RenderViewToString("MailTemplates/RegisterConfirmation", mailViewModel);

                _emailService.SendEmail(user.Email, "DevDay 2015 CFP - Account Activation", registrationMailContent);

                return new RedirectResponse("/");
            };

            Get["/logout"] = parameters => this.LogoutAndRedirect("/");

            Get["/remindpassword"] = parameters =>
            {
                if (Context.CurrentUser != null)
                {
                    return new RedirectResponse("/");
                }
                return View["RemindPassword"];
            };

            Post["/remindpassword"] = parameters =>
            {
                string email = Request.Form["Email"];

                var userData = dataStore.GetUserByUsernameOrEmail(null, email);
                if (userData == null)
                {
                    ViewBag.Page.Value.Errors.Add(new ErrorViewModel
                    {
                        ErrorMessage = "Sorry, but we don't have any account with this email address",
                        Name = "Email"
                    });

                    return View["RemindPassword"];
                }

                SendResetPasswordToken(userData);

                return View["RemindPasswordMessageSent"];
            };

            Get["/resetpassword/{token}"] = parameters =>
            {
                if (Context.CurrentUser != null)
                {
                    return new RedirectResponse("/");
                }

                Guid tokenUID;
                if (Guid.TryParse(parameters.token, out tokenUID))
                {
                    var token = _dataStore.FindTokenByContent(tokenUID);
                    if (token != null && token.IsActive)
                    {
                        var timeDifference = DateTime.UtcNow - token.CreateDate;
                        if (timeDifference < TimeSpan.FromDays(1))
                        {
                            var model = new ResetPasswordViewModel();
                            model.UserId = token.User.Id;
                            model.TokenId = token.Id;
                            return View["RemindPasswordReset", model];
                        }
                    }
                }

                return View["RemindPasswordInvalidToken"];
            };

            Post["/resetpassword/{token}"] = parameters =>
            {
                var model = this.Bind<ResetPasswordViewModel>();
                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["RemindPasswordReset", model];
                }

                var user = _dataStore.GetUserById(model.UserId);
                user.Password = Helpers.EncodePassword(model.Password);

                var token = _dataStore.GetTokenById(model.TokenId);
                token.IsActive = false;

                using (var transaction = _dataStore.DatabaseObject.BeginTransaction())
                {
                    _dataStore.SaveUser(user);
                    _dataStore.SaveToken(token);

                    transaction.Commit();
                }

                return new RedirectResponse("/Account/Login");
            };

            Get["/register"] = parameters =>
            {
                var registerModel = new RegisterViewModel();

                return View["Register", registerModel];
            };

            Post["/register"] = parameters =>
            {
                var model = this.Bind<RegisterViewModel>();
                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Register", model];
                }

                var userMapper = new UserMapper(dataStore);
                var userData = userMapper.ValidateRegisterNewUser(model);

                if (userData == null)
                {
                    ViewBag.Page.Value.Errors.Add(new ErrorViewModel
                    {
                        ErrorMessage = "Username or email already in use. Please choose different one.",
                        Name = "Username"
                    });

                    return View["Register", model];
                }

                var mailViewModel = new RegisterMailViewModel();
                mailViewModel.User = userData;
                mailViewModel.Hostname = BuildHostname(Context.Request.Url);
                var registrationMailContent = this.RenderViewToString("MailTemplates/RegisterConfirmation", mailViewModel);

                _emailService.SendEmail(userData.Email, "DevDay 2015 CFP - Account Activation", registrationMailContent);

                DateTime? expiry = DateTime.Now.AddDays(7);

                return this.LoginAndRedirect(userData.Id, expiry, "/papers");
            };

            Get["/profile"] = parameters =>
            {
                this.RequiresAuthentication();

                var userId = Context.CurrentUser.GetId();
                var userEntity = dataStore.GetUserById(userId);

                string avatarPath = !String.IsNullOrEmpty(userEntity.AvatarPath)
                                ? String.Format("/Images/Avatars/{0}/{1}", userEntity.Id, userEntity.AvatarPath)
                                : String.Format("https://www.gravatar.com/avatar/{0}?s=100&d=mm", userEntity.EmailHash.ToLower());

                ViewBag.AvatarPath = avatarPath;


                return View["Profile", userEntity];
            };

            Post["/profile"] = parameters =>
            {
                var userModel = this.Bind<User>();
                var result = this.Validate(userModel);

                bool shouldResetAvatar = Request.Form["resetAvatar"];
                string avatarData = Request.Form["AvatarData"];

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Profile", userModel];
                }

                var userId = Context.CurrentUser.GetId();
                var userFromDb = dataStore.GetUserById(userId);
                userFromDb.Bio = (userModel.Bio ?? String.Empty).Trim();
                userFromDb.Email = userModel.Email;
                userFromDb.Location = userModel.Location;
                userFromDb.Name = userModel.Name;
                userFromDb.TwitterHandle = userModel.TwitterHandle;
                userFromDb.Website = userModel.Website;
                userFromDb.ShowOff = userModel.ShowOff;

                var avatarFile = Request.Files.FirstOrDefault();

                if (avatarFile != null && avatarFile.Name != null && !String.IsNullOrEmpty(avatarData))
                {
                    var folderPath = Path.Combine(pathProvider.GetRootPath(), "Images/Avatars/" + userId);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, Path.GetFileNameWithoutExtension(avatarFile.Name)) + ".png";

                    SaveImageFromBase64Data(avatarData, filePath);

                    userFromDb.AvatarPath = Path.GetFileName(filePath);
                }

                if (shouldResetAvatar)
                {
                    userFromDb.AvatarPath = String.Empty;
                }

                dataStore.SaveUser(userFromDb);

                return Response.AsRedirect("/papers");
            };

            Get["/activate/{token}"] = parameters =>
            {
                this.RequiresAuthentication();

                var loggedUser = (User)Context.CurrentUser;

                if (loggedUser.RegistrationToken == parameters.token)
                {
                    var user = dataStore.GetUserById(loggedUser.Id);
                    user.AccountStatus = AccountStatus.Active;
                    dataStore.SaveUser(user);

                    return Response.AsRedirect("/activated");
                }
                return Response.AsRedirect("/keyfailed");
            };
        }

        private string BuildHostname(Url url)
        {
            var hostname = url.Scheme + "://" + url.HostName;
            if(url.Port.HasValue && (url.Port.Value != 80 && url.Port.Value != 443))
            {
                hostname += ":" + url.Port;
            }
            return hostname;
        }

        private void SendResetPasswordToken(User userData)
        {
            var token = new Token(userData, TokenType.PasswordResetToken);
            token.TokenGuid = Guid.NewGuid();

            _dataStore.SaveToken(token);

            var resetMailViewModel = new ResetPasswordMailViewModel();
            resetMailViewModel.Token = token.TokenGuid;
            resetMailViewModel.HostName = BuildHostname(Context.Request.Url);

            var mailBody = this.RenderViewToString("MailTemplates/ResetPassword", resetMailViewModel);
            _emailService.SendEmail(userData.Email, "Reset password", mailBody);
        }

        private static void SaveImageFromBase64Data(string avatarData, string filePath)
        {
            int indexOfBase64Mark = avatarData.IndexOf(";base64,", StringComparison.Ordinal);
            if (indexOfBase64Mark == -1)
            {
                throw new ArgumentException("Given string is not valid Base64 encoded image from form");
            }

            avatarData = avatarData.Substring(indexOfBase64Mark + ";base64,".Length);

            var bytes = Convert.FromBase64String(avatarData);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
        }
    }
}