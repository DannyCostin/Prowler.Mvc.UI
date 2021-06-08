using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prowler.Mvc.UI.Global
{
    internal static class ProwlerHelper
    {
        internal static T GetPropValue<T>(object src, string propName)
        {
            if (src == null)
            {
                return default;
            }

            try
            {
                object result;

                if (IsSubProperty(propName))
                {
                    result = GetSubPropValue<T>(src, propName);
                }
                else
                {
                    result = GetPropValueSingle<T>(src, propName);
                }

                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        private static T GetPropValueSingle<T>(object src, string propName)
        {
            if (src == null)
            {
                return default;
            }

            try
            {
                var result = src.GetType().GetProperty(propName ?? string.Empty)
                                       ?.GetValue(src, null);

                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        private static object GetPropValueObject(object src, string propName)
        {
            if (src == null)
            {
                return null;
            }

            try
            {
                return src.GetType().GetProperty(propName ?? string.Empty)
                                   ?.GetValue(src, null);                
            }
            catch
            {
                return null;
            }
        }

        private static T GetSubPropValue<T>(object src, string propName)
        {
            if (src == null)
            {
                return default;
            }

            try
            {
                var propertyList = GetSubProperties(propName);
                dynamic prop = src;                

                foreach(var item in propertyList)
                {
                    prop = GetPropValueObject(prop, item);
                }

                return prop;
            }
            catch
            {
                return default;
            }
        }

        private static List<string> GetSubProperties(string property)
        {
            if (property == null)
            {
                return new List<string>();
            }

            return property.Split('.').ToList();
        }

        private static bool IsSubProperty(string propertyName)
        {
            var propertyList = GetSubProperties(propertyName);

            if (propertyList.Any() && propertyList.Count > 1)
            {
                return true;
            }

            return false;
        }
    }
}
