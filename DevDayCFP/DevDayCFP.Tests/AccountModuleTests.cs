using System;
using DevDayCFP.Modules;
using DevDayCFP.Services;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace DevDayCFP.Tests
{
    public class AccountModuleTests
    {
        private readonly Browser _browser;
        private readonly IDataStore _dataStoreMock;
        private readonly IEmailService _emailServiceMock;
        private readonly DefaultRootPathProvider _rootPathProvider;

        public AccountModuleTests()
        {
            _dataStoreMock = A.Fake<IDataStore>();
            _emailServiceMock = A.Fake<IEmailService>();
            _rootPathProvider = new DefaultRootPathProvider();

            var bootstrapper = new ConfigurableBootstrapper(with =>
            {
                with.Module(new AccountModule(_dataStoreMock, _emailServiceMock, _rootPathProvider));
                with.ViewFactory<TestingViewFactory>();
                with.RootPathProvider(_rootPathProvider);
            });
            _browser = new Browser(bootstrapper);
        }

        [Fact]
        public void Should_Show_MessageSentView_After_Proper_RemindPasswordCall()
        {
            var response = _browser.Post("/account/remindpassword", (with) =>
            {
                with.HttpRequest();
                with.FormValue("Email", "mail@test.pl");
            });

            Assert.Equal("RemindPasswordMessageSent", response.GetViewName());
        }

        [Fact]
        public void Should_Return_To_The_Same_View_If_RemindPassword_Called_With_Wrond_Email()
        {
            A.CallTo(() => _dataStoreMock.GetUserByUsernameOrEmail(A<string>.Ignored, A<string>.Ignored)).Returns(null);

            var response = _browser.Post("/account/remindpassword", (with) =>
            {
                with.HttpRequest();
                with.FormValue("Email", "mail@test.pl");
            });

            Assert.Equal("RemindPassword", response.GetViewName());
        }
    }
}
