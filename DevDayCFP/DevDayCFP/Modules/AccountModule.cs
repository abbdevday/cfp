using System;
using DevDayCFP.Services;
using DevDayCFP.ViewModels;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Validation;

namespace DevDayCFP.Modules
{
    public class AccountModule : BaseModule
    {
        public AccountModule(IDataStore dataStore) : base("account")
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

                var userMapper = new UserMapper(dataStore);
                var userGuid = userMapper.ValidateUser(model.UserName, model.Password);

                if (userGuid == null || !result.IsValid)
                {
                    // TODO: Add error indication
                    return View["LogOn", model];
                }

                DateTime? expiry = null;
                if (model.RememberMe)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
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
                    return View["Register", model];
                }

                var userMapper = new UserMapper(dataStore);
                var userGUID = userMapper.ValidateRegisterNewUser(model);

                if (userGUID == null)
                {
                    return View["Register", model];
                }

                DateTime? expiry = DateTime.Now.AddDays(7);

                return this.LoginAndRedirect(userGUID.Value, expiry);
            };
        }
    }
}