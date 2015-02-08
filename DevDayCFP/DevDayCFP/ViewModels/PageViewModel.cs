using System.Collections.Generic;

namespace DevDayCFP.ViewModels
{
    public class PageViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string CurrentUser { get; set; }
        public List<ErrorViewModel> Errors { get; private set; }

        public PageViewModel()
        {
            Errors = new List<ErrorViewModel>();
        }
    }
}