using System;
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
    }
}