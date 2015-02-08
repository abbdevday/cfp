using System.Collections.Generic;
using System.Linq;
using DevDayCFP.ViewModels;
using Nancy.Validation;

namespace DevDayCFP.Extensions
{
    public static class ValidationExtensions
    {
        public static IEnumerable<ErrorViewModel> AsErrorViewModels(this ModelValidationResult result)
        {
            foreach (var item in result.Errors.SelectMany(e => e.Value))
            {
                foreach (var member in item.MemberNames)
                {
                    yield return new ErrorViewModel() {Name = member, ErrorMessage = item.ErrorMessage};
                }
            }
        }
    }
}