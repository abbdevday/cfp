using System.Collections.Generic;

namespace DevDayCFP.Common
{
    public static class Consts
    {
        public static readonly IEnumerable<int> PaperLevels = new List<int> {100, 200, 300, 400, 500};

        public static class SettingKeys
        {
            public const string PassKeyGeneratorSalt = "PassKeyGeneratorSalt";
            public const string EncryptionKeyGeneratorPass = "EncryptionKeyGeneratorPass";
            public const string HmacKeyGeneratorPass = "HmacKeyGeneratorPass";
            public const string PasswordSalt = "PasswordSalt";
        }

        public static class Claims
        {
            public const string User = "User";
            public const string Admin = "Admin";
        }
    }
}