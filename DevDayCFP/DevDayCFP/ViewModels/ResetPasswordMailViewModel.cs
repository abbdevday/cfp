using System;

namespace DevDayCFP.ViewModels
{
    public class ResetPasswordMailViewModel
    {
        public Guid Token { get; set; }
        public string HostName { get; set; }
    }
}