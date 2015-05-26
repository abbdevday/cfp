using System;
using System.Linq;
using DevDayCFP.Common;
using DevDayCFP.Extensions;
using DevDayCFP.Models;
using DevDayCFP.Services;
using DevDayCFP.ViewModels;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Nancy.Validation;

namespace DevDayCFP.Modules
{
    public class PapersModule : BaseModule
    {
        private readonly IEmailService _emailService;

        public PapersModule(IDataStore dataStore, IEmailService emailService) : base("papers", dataStore)
        {
            _emailService = emailService;
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var papersList = dataStore.GetPapersByUser(Context.CurrentUser.GetId());

                return View["Index", papersList];
            };

            Get["/add"] = _ => View["Edit", new Paper { User = (User)Context.CurrentUser }];
            Post["/add"] = parameters =>
            {
                var paper = this.Bind<Paper>();
                var result = this.Validate(paper);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Edit", paper];
                }

                paper.User = (User)Context.CurrentUser;
                paper.Id = Guid.NewGuid();
                dataStore.SavePaper(paper);
                var newPaperVm = new NewPaperSubmittedViewModel {Paper = paper};
                var adminEmails = dataStore.GetAdmins().Select(x => x.Email);

                var mailBody = this.RenderViewToString("MailTemplates/NewPaperSubmitted", newPaperVm);
                _emailService.SendEmail(adminEmails, "New paper submitted", mailBody);

                return Response.AsRedirect("/papers");
            };

            Get["/edit/{id}"] = parameters =>
            {
                if (!parameters.id.HasValue && String.IsNullOrWhiteSpace(parameters.id))
                {
                    return 404;
                }

                var paper = dataStore.GetPaperById((Guid)parameters.id);

                if (paper == null)
                {
                    return 404;
                }

                return View["Edit", paper];
            };

            Post["/edit/{id}"] = parameters =>
            {
                var paper = this.Bind<Paper>();
                var result = this.Validate(paper);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Edit", paper];
                }

                paper.LastModificationDate = DateTime.UtcNow;
                paper.User = (User)Context.CurrentUser;
                dataStore.SavePaper(paper);

                return Response.AsRedirect("/papers");
            };

            Post["/delete/{id}"] = parameters =>
            {
                Paper paper = dataStore.GetPaperById(parameters.id);

                if (paper == null)
                {
                    return 404;
                }

                paper.IsActive = false;
                paper.LastModificationDate = DateTime.UtcNow;
                paper.User = (User)Context.CurrentUser;
                dataStore.SavePaper(paper);

                return HttpStatusCode.OK;
            };
        }
    }
}