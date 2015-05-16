using System;

namespace DevDayCFP.Models
{
    public class Token
    {
        public Guid Id { get; set; }

        public User User { get; set; }

        public Guid TokenGuid { get; set; }

        public TokenType Type { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

        public Token()
        {
            
        }

        public Token(User user, TokenType tokenType)
        {
            Id = Guid.NewGuid();
            User = user;
            Type = tokenType;
            CreateDate = DateTime.UtcNow;
            IsActive = true;
        }
    }

    public enum TokenType : int
    {
        PasswordResetToken = 1
    }
}