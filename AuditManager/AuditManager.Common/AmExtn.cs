using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace AuditManager.Common
{
    public enum UserNameType
    {
        First = 1,
        Last,
    }

    public static class AmExtn
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }

        public static string ToJString(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static dynamic ToJObject(this string value)
        {
            //JsonConvert.DeserializeAnonymousType<T
            return string.Empty;
        }

        public static int ToInt(this string value, int? defaultValue = null)
        {
            int i;
            if (int.TryParse(value, out i))
                return i;

            return defaultValue.GetValueOrDefault(0);

        }

        public static T ToDate<T>(this string value, DateTime? defaultValue = null)
        {
            var uT = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            DateTime dt;
            if (DateTime.TryParse(value, out dt))
                return (T)Convert.ChangeType(dt, uT);

            return defaultValue == null ? default(T) : (T)Convert.ChangeType(defaultValue, uT);
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            if (typeof(T).IsEnum)
            {
                T outT;
                if (Enum.TryParse<T>(value, true, out outT))
                {
                    return outT;
                }
            }

            return default(T);
        }

        public static string ToEnumDesc<T>(this T value) where T : struct
        {
            if (typeof(T).IsEnum)
            {
                var descAttrs = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descAttrs != null && descAttrs.Length > 0)
                    return descAttrs[0].Description;
            }

            return value.ToString();
        }

        public static U GetPropAttr<T, U>(this T value, Expression<Func<T, string>> property)
        {
            if (value == null || property == null)
                return default(U);

            var pInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
            return (U)pInfo.GetCustomAttributes(typeof(U), false)[0];

            //var pBody = property.Body;
            //if (pBody == null) return default(U);
            //var pInfo = ((MemberExpression)pBody).Member as PropertyInfo;
            //if (pInfo == null) return default(U);
            //var ret = pInfo.GetCustomAttributes(typeof(U), false);
            //if (ret == null) return default(U);
            //return (U)ret[0];
        }

        public static T FromBool<T, U>(this U value)
        {
            return value.ToBool<U>().FromBool<T>();
        }

        public static bool ToBool<T>(this T value)
        {
            if (value == null)
                return false;

            if (typeof(T) == typeof(bool))
            {
                return (bool)Convert.ChangeType(value, typeof(bool));
            }
            else if (typeof(T) == typeof(int))
            {
                return (int)Convert.ChangeType(value, typeof(int)) == 1;
            }
            else if (typeof(T) == typeof(char))
            {
                char c = (char)Convert.ChangeType(value, typeof(char));

                if (char.ToUpper(c).Equals('Y') || char.ToUpper(c).Equals('T'))
                    return true;
                else
                    return false;
            }
            else if (typeof(T) == typeof(string))
            {
                string s = (string)Convert.ChangeType(value, typeof(string));

                if (string.IsNullOrWhiteSpace(s))
                    return false;

                s = s.Trim().ToUpper();

                if (s.Equals("Y") || s.Equals("T") || s.Equals("YES") || s.Equals("TRUE") || s.Equals("OK") || s.Equals("ON") || s.Equals("1"))
                    return true;
                else
                    return false;
            }

            return false;
        }

        public static T FromBool<T>(this bool value)
        {
            if (typeof(T) == typeof(int))
            {
                return value ? (T)Convert.ChangeType(1, typeof(int)) : (T)Convert.ChangeType(0, typeof(int));
            }
            else if (typeof(T) == typeof(char))
            {
                return value ? (T)Convert.ChangeType("Y", typeof(char)) : (T)Convert.ChangeType("N", typeof(char));
            }

            return (T)Convert.ChangeType(value, typeof(bool));
        }

        public static string UserName(this string value, UserNameType userNameType)
        {
            var spName = value.Split(',');

            if (spName.Length > 1)
            {
                if (userNameType == UserNameType.Last)
                    return spName[0];
                else
                    return spName[1];
            }
            else
                return value;
        }

        public static string FileExtn(this string value)
        {
            var sp = value.Split('.');

            return sp[sp.Length - 1];
        }

        public static string FileNameWithExtn(this string value, string extn)
        {
            if (value.FileExtn().Equals(extn, StringComparison.OrdinalIgnoreCase))
                return value;
            else
                return string.Format("{0}.{1}", value, extn);
        }

        public static string FileType(this string value, string extn)
        {
            if (string.IsNullOrWhiteSpace(extn))
            {

            }
            else if (extn.Length < 3)
            {

            }

            if (value.FileExtn().Equals(extn, StringComparison.OrdinalIgnoreCase))
                return extn.Length < 3 ? extn : extn.Substring(0, 3);
            else
            {
                return extn.Length < 3 ? extn : extn.Substring(0, 3);
            }
        }

        public static string SplitNGet(this string value, char spliter, int pos)
        {
            var s = value.Split(spliter);
            if (s.Length > pos)
                return s[pos];

            return null;
        }

        public static string ToEmail(this string value)
        {
            return string.Format("{0}@kpmg.com", value.SplitNGet('@', 0));
        }

        public static string ToKPMGEmail(this string value)
        {
            return string.Format("{0}@kpmg.com", value);
        }

        public static DateTime ToUTCAdjustment(this DateTime value)
        {
            if (value == DateTime.MinValue)
                value = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            
            return value.AddDays(1).AddSeconds(-1).ToUniversalTime();
        }

        public static string ToUserIdFromDnsName(this string value)
        {
            return value.SplitNGet('\\', 1);
        }

        public static string ToUserIdWithDomainName(this string value)
        {
            return string.Format("{0}\\{1}", ConfigUtility.GetDomainName(), value);
        }
    }
}
