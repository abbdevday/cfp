using System;
using DevDayCFP.Models;
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

            A.CallTo(() => _dataStoreMock.GetUtcClosingTime()).Returns(DateTime.MaxValue);
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

        [Fact]
        public void Should_Save_New_Token_To_DB_With_Correct_User_On_ResetPassword()
        {
            var testUser = new User() { Id = Guid.NewGuid() };
            A.CallTo(() => _dataStoreMock.GetUserByUsernameOrEmail(A<string>.Ignored, A<string>.Ignored)).Returns(testUser);

            var response = _browser.Post("/account/remindpassword", (with) =>
            {
                with.HttpRequest();
                with.FormValue("Email", "mail@test.pl");
            });

            A.CallTo(() => _dataStoreMock.SaveToken(A<Token>.That.Matches(t => ReferenceEquals(t.User, testUser)))).MustHaveHappened();
        }

        [Fact]
        public void Should_Save_New_Token_To_DB_With_Correct_Type_On_ResetPassword()
        {
            var testUser = new User() { Id = Guid.NewGuid() };
            A.CallTo(() => _dataStoreMock.GetUserByUsernameOrEmail(A<string>.Ignored, A<string>.Ignored)).Returns(testUser);

            var response = _browser.Post("/account/remindpassword", (with) =>
            {
                with.HttpRequest();
                with.FormValue("Email", "mail@test.pl");
            });

            A.CallTo(() => _dataStoreMock.SaveToken(A<Token>.That.Matches(t => t.Type == TokenType.PasswordResetToken))).MustHaveHappened();
        }

        [Fact]
        public void Should_Show_InvalidToken_Page_When_Token_NotFound()
        {
            var testGuid = Guid.NewGuid();

            A.CallTo(() => _dataStoreMock.GetTokenById(testGuid)).Returns(null);

            var response = _browser.Get("/account/resetpassword/" + testGuid, (with) =>
            {
                with.HttpRequest(); ;
            });

            Assert.Equal("RemindPasswordInvalidToken", response.GetViewName());
        }

        [Fact]
        public void Should_Show_InvalidToken_Page_When_Token_NotActive()
        {
            var testGuid = Guid.NewGuid();
            var testUser = new User() { Id = Guid.NewGuid() };
            var testToken = new Token(testUser, TokenType.PasswordResetToken)
            {
                Id = testGuid,
                IsActive = false
            };

            A.CallTo(() => _dataStoreMock.GetTokenById(testGuid)).Returns(testToken);

            var response = _browser.Get("/account/resetpassword/" + testGuid, (with) =>
            {
                with.HttpRequest(); ;
            });

            Assert.Equal("RemindPasswordInvalidToken", response.GetViewName());
        }

        [Fact]
        public void Should_Show_InvalidToken_Page_When_Token_Expired()
        {
            var testGuid = Guid.NewGuid();
            var testUser = new User() { Id = Guid.NewGuid() };
            var testToken = new Token(testUser, TokenType.PasswordResetToken)
            {
                Id = testGuid,
                CreateDate = DateTime.UtcNow.AddDays(-1).AddMinutes(-1)
            };

            A.CallTo(() => _dataStoreMock.GetTokenById(testGuid)).Returns(testToken);

            var response = _browser.Get("/account/resetpassword/" + testGuid, (with) =>
            {
                with.HttpRequest(); ;
            });

            Assert.Equal("RemindPasswordInvalidToken", response.GetViewName());
        }
    }
}
