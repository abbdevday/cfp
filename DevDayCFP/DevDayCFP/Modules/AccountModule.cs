using System;
using DevDayCFP.Extensions;
using DevDayCFP.Services;
using DevDayCFP.ViewModels;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Validation;

namespace DevDayCFP.Modules
{
    public class AccountModule : BaseModule
    {
        public AccountModule(IDataStore dataStore)
            : base("account")
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
                var userGuid = userMapper.ValidateRegisterNewUser(model);

                if (userGuid == null)
                {
                    ViewBag.Page.Value.Errors.Add(new ErrorViewModel
                    {
                        ErrorMessage = "Username or email already in use. Please choose different one.",
                        Name = "Username"
                    });
                   
                    return View["Register", model];
                }

                DateTime? expiry = DateTime.Now.AddDays(7);

                return this.LoginAndRedirect(userGuid.Value, expiry, "/papers");
            };
        }
    }
}