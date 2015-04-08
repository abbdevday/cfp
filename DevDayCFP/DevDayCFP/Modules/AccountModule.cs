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
        public AccountModule(IDataStore dataStore, IEmailService emailService, IRootPathProvider pathProvider)
            : base("account", dataStore)
        {
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

                return this.LoginAndRedirect(userGuid.Value, expiry, "/papers");
            };

            Get["/logout"] = parameters => this.LogoutAndRedirect("/");


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

                string hostName = Context.Request.Url.SiteBase;
                emailService.SendRegistrationEmail(userData, hostName);

                DateTime? expiry = DateTime.Now.AddDays(7);

                return this.LoginAndRedirect(userData.Id, expiry, "/papers");
            };

            Get["/profile"] = parameters =>
            {
                this.RequiresAuthentication();

                var userId = Context.CurrentUser.GetId();
                var userEntity = dataStore.GetUserById(userId);


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

                var loggedUser = (User) Context.CurrentUser;

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