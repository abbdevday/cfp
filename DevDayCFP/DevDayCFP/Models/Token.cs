using System;
using System.ComponentModel.DataAnnotations;

namespace DevDayCFP.Models
{
    public class Token
    {
        public Guid Id { get; set; }

        public User User { get; set; }

        public Guid TokenGuid { get; set; }

        public TokenType TokenType { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

        public Token()
        {
            IsActive = true;
        }
    }

    public enum TokenType : int
    {
        PasswordResetToken = 1
    }
}