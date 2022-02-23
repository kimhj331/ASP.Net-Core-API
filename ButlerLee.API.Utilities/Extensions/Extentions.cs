using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace ButlerLee.API.Extensions
{
    public static class Extentions
    {
        public static int ToInt(this string number, int defaultInt = 0)
        {
            int resultNum = defaultInt;

            try
            {
                if (!string.IsNullOrEmpty(number))
                    resultNum = Convert.ToInt32(number);
            }
            catch { }
            return resultNum;
        }

        public static int ToInt(this object number, int defaultInt = 0)
        {
            int resultNum = defaultInt;

            try
            {
                if (number == null)
                    resultNum = defaultInt;
                else
                    resultNum = Convert.ToInt32(number);
            }
            catch { }
            return resultNum;
        }

        public static int ToInt(this int? number, int defaultInt = 0)
        {
            int resultNum = defaultInt;

            try
            {
                if (number == null)
                    resultNum = defaultInt;
                else
                    resultNum = Convert.ToInt32(number);
            }
            catch { }
            return resultNum;
        }


        public static short ToShort(this string number, short defaultInt = 0)
        {
            short resultNum = defaultInt;

            try
            {
                if (!string.IsNullOrEmpty(number))
                    resultNum = Convert.ToInt16(number);
            }
            catch { }
            return resultNum;
        }

        public static short ToShort(this object number, short defaultInt = 0)
        {
            short resultNum = defaultInt;

            try
            {
                if (number == null)
                    resultNum = defaultInt;
                else
                    resultNum = Convert.ToInt16(number);
            }
            catch { }
            return resultNum;
        }

        public static short ToShort(this short? number, short defaultInt = 0)
        {
            short resultNum = defaultInt;

            try
            {
                if (number == null)
                    resultNum = defaultInt;
                else
                    resultNum = Convert.ToInt16(number);
            }
            catch { }
            return resultNum;
        }

        public static ulong ToULong(this string number, ulong defaultInt = 0)
        {
            ulong resultNum = defaultInt;

            try
            {
                if (!string.IsNullOrEmpty(number))
                    resultNum = Convert.ToUInt64(number);
            }
            catch { }
            return resultNum;
        }

        public static ulong ToULong(this object number, ulong defaultInt = 0)
        {
            ulong resultNum = defaultInt;

            try
            {
                if (number == null)
                    resultNum = defaultInt;
                else
                    resultNum = Convert.ToUInt64(number);
            }
            catch { }
            return resultNum;
        }

        public static ulong ToULong(this ulong? number, ulong defaultInt = 0)
        {
            ulong resultNum = defaultInt;

            try
            {
                if (number == null)
                    resultNum = defaultInt;
                else
                    resultNum = Convert.ToUInt64(number);
            }
            catch { }
            return resultNum;
        }

        public static DateTime StartOfDay(this DateTime d)
        {
            return DateTime.Parse(d.ToShortDateString().Trim() + " 00:00:00");
        }

        public static DateTime EndOfDay(this DateTime d)
        {
            return DateTime.Parse(d.ToShortDateString().Trim() + " 23:59:59");
        }

        public static DateTime? ToDateTime(this object value, DateTime? datetime = null)
        {
            DateTime? result = datetime;
            try
            {
                DateTime tempDate;
                if (DateTime.TryParse(value.ToString(), out tempDate) == false)
                    return datetime;

                return tempDate;
            }
            catch { }
            return result;
        }

        public static DateTime ToDateTime(this string value, string format)
        {
            string[] formats = { format };

            DateTime result;
            DateTime.TryParseExact(value,
                        formats,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out result);

            return result;
        }

        public static DateTime Truncate(this DateTime date, long resolution)
        {
            return new DateTime(date.Ticks - (date.Ticks % resolution), date.Kind);
        }

        public static DateTime AddBusinessDays(this DateTime source, int businessDays)
        {
            int dayOfWeek = businessDays < 0
                                ? ((int)source.DayOfWeek - 12) % 7
                                : ((int)source.DayOfWeek + 6) % 7;

            switch (dayOfWeek)
            {
                case 6:
                    businessDays--;
                    break;
                case -6:
                    businessDays++;
                    break;
            }

            return source.AddDays(businessDays + ((businessDays + dayOfWeek) / 5) * 2);
        }


        public static decimal ToDecimal(this object number, decimal defaultInt = 0)
        {
            decimal resultNum = defaultInt;

            try
            {
                if (number == null)
                    resultNum = defaultInt;
                else
                    resultNum = Convert.ToDecimal(number);
            }
            catch { }
            return resultNum;
        }

        public static decimal ToDecimal(this string number, decimal defaultInt = 0)
        {
            decimal resultNum = defaultInt;

            try
            {
                if (!string.IsNullOrEmpty(number))
                    resultNum = Convert.ToDecimal(number);
            }
            catch { }
            return resultNum;
        }

        public static double ToEpochDateHighCharts(this DateTime date)
        {
            TimeSpan t = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return t.TotalMilliseconds;
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static string GetDisplayName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DisplayAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute))
                        as DisplayAttribute;

            return attribute == null ? value.ToString() : attribute.Name;
        }

        public static string ToJson(this object value)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
            return JsonConvert.SerializeObject(value, settings);
        }

        public static string ToSnakeCase(this string str) =>
        string.Concat(str.Select((x, i) => (i > 0 && char.IsUpper(x) && (char.IsLower(str[i - 1]) || char.IsLower(str[i + 1])))
            ? "_" + x.ToString() : x.ToString())).ToLower();
    }
}
