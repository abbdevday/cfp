using System;
using System.Collections.Generic;
using System.Linq;
using DevDayCFP.ViewModels;
using Nancy.ViewEngines.Razor;

namespace DevDayCFP.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString CheckBox<T>(this HtmlHelpers<T> helper, string name, dynamic modelProperty)
        {
            string input = String.Empty;
            bool checkedState = false;

            if (!bool.TryParse(modelProperty.ToString(), out checkedState))
            {
                input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" />";
            }
            else
            {
                if (checkedState)
                    input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" checked />";
                else
                    input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" />";
            }


            return new NonEncodedHtmlString(input);
        }

        public static IHtmlString ValidationSummary<T>(this HtmlHelpers<T> helper, List<ErrorViewModel> errors)
        {
            if (!errors.Any())
            {
                return new NonEncodedHtmlString("");
            }

            string div = errors.Aggregate("<div class=\"validation-summary-errors\"><span>Below form is not valid. Please correct the errors and try again.</span><ul>",
                                            (current, item) => current + ("<li>" + item.ErrorMessage + "</li>"));

            div += "</ul></div>";

            return new NonEncodedHtmlString(div);
        }
    }
}