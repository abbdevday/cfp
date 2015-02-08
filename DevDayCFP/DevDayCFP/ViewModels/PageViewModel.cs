using System.Collections.Generic;

namespace DevDayCFP.ViewModels
{
    public class PageViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string CurrentUser { get; set; }
        public List<ErrorViewModel> Errors { get; set; }

        public override string ToString()
        {
            return "ToString" + IsAuthenticated.ToString();
        }
    }
}