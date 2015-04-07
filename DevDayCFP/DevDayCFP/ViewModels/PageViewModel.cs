using System.Collections.Generic;

namespace DevDayCFP.ViewModels
{
    public class PageViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string CurrentUser { get; set; }
        public string EmailHash { get; set; }
        public string AvatarPath { get; set; }
        public List<ErrorViewModel> Errors { get; private set; }

        public int NoOfUsers { get; set; }
        public int NoOfPapers { get; set; }

        public PageViewModel()
        {
            Errors = new List<ErrorViewModel>();
        }
    }
}