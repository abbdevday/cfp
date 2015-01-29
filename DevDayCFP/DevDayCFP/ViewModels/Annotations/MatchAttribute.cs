using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DevDayCFP.ViewModels.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MatchAttribute : ValidationAttribute
    {
        public String SourceProperty { get; set; }
        public String MatchProperty { get; set; }

        public MatchAttribute(string source, string match)
        {
            SourceProperty = source;
            MatchProperty = match;
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage;
        }

        public override Boolean IsValid(Object value)
        {
            if (value == null)
                return false;

            Type objectType = value.GetType();

            PropertyInfo[] properties = objectType.GetProperties();

            var sourceValue = new object();
            var matchValue = new object();

            Type sourceType = null;
            Type matchType = null;

            int counter = 0;

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.Name == SourceProperty || propertyInfo.Name == MatchProperty)
                {
                    if (counter == 0)
                    {
                        sourceValue = propertyInfo.GetValue(value, null);
                        if (sourceValue != null)
                            sourceType = propertyInfo.GetValue(value, null).GetType();
                    }
                    if (counter == 1)
                    {
                        matchValue = propertyInfo.GetValue(value, null);
                        if (matchValue != null)
                            matchType = propertyInfo.GetValue(value, null).GetType();
                    }
                    counter++;
                    if (counter == 2)
                    {
                        break;
                    }
                }
            }

            if (sourceType != null && matchType != null)
            {
                return String.Equals(sourceValue.ToString(), matchValue.ToString());
            }
            return false;
        }
    }
}