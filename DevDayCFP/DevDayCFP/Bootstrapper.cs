using System;
using System.Configuration;
using System.Text;
using DevDayCFP.Common;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Cryptography;
using Nancy.TinyIoc;
using NLog;

namespace DevDayCFP
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", @"Scripts"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("images", @"Images"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("fonts", @"Fonts"));
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            _logger.Info("Application starting");
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                DbMigrationRunner.MigrateToLatest(connectionString);

                _logger.Info("Migrations applied if needed");


                base.ApplicationStartup(container, pipelines);

                var cryptographyConfiguration = GetCryptographyConfiguration();

                var formsAuthConfiguration = new FormsAuthenticationConfiguration
                {
                    CryptographyConfiguration = cryptographyConfiguration,
                    RedirectUrl = "~/account/login",
                    UserMapper = container.Resolve<IUserMapper>(),
                };

                FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
                throw;
            }
        }

        private static CryptographyConfiguration GetCryptographyConfiguration()
        {
            string salt = ConfigurationManager.AppSettings[Consts.SettingKeys.PassKeyGeneratorSalt];
            string encryptionKey = ConfigurationManager.AppSettings[Consts.SettingKeys.EncryptionKeyGeneratorPass];
            string hmacKey = ConfigurationManager.AppSettings[Consts.SettingKeys.HmacKeyGeneratorPass];

            var saltAsBytes = Encoding.ASCII.GetBytes(salt);

            return new CryptographyConfiguration(
                new RijndaelEncryptionProvider(new PassphraseKeyGenerator(encryptionKey, saltAsBytes)),
                new DefaultHmacProvider(new PassphraseKeyGenerator(hmacKey, saltAsBytes)));
        }
    }
}