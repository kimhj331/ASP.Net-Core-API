using ButlerLee.API.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace ButlerLee.API.Models.Filters
{
    public class BaseParameters
    {
        public string GetQueryString(string separator = ",")
        {
            var properties = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                //.Where(x => x.CanRead)
                .Where(x => x.GetValue(this, null) != null)
                .Where(x => x.GetCustomAttribute<JsonPropertyAttribute>() != null)
                .ToDictionary(x => x.GetCustomAttribute<JsonPropertyAttribute>() == null ?
                                   x.Name :
                                   x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName, 
                                   x => 
                                   {
                                       if (x.GetValue(this, null) is DateTime)
                                           return HttpUtility.UrlEncode(((DateTime)x.GetValue(this, null)).ToString("yyyy-MM-dd"));
                                       else
                                           return x.GetValue(this, null);
                                   });


            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is ICollection)
                .Select(x => x.Key)
                .ToList();
            
            string queryString = string.Join("&", properties.Where(o => propertyNames.Contains(o.Key) == false)
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));

            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                                        ? valueType.GetGenericArguments()[0]
                                        : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    foreach (var item in enumerable)
                    {
                        queryString += $"&{key}[]={Uri.EscapeDataString(item.ToString())}";
                    }
                }
            }

            return queryString;
        }
        public Dictionary<string, string> GetDictionary()
        {
            Dictionary<string, string> properties = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    //.Where(x => x.CanRead)
                    .Where(x => x.GetValue(this, null) != null)
                    .Where(x => x.GetCustomAttribute<JsonPropertyAttribute>() != null)
                    .ToDictionary(x => x.GetCustomAttribute<JsonPropertyAttribute>() == null ?
                                       x.Name.ToString() :
                                       x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName.ToString(),
                                       x =>
                                       {
                                           if (x.GetValue(this, null) is DateTime)
                                               return HttpUtility.UrlEncode(((DateTime)x.GetValue(this, null)).ToString("yyyy-MM-dd")).ToString();
                                           else
                                               return x.GetValue(this, null).ToString();
                                       });
            return properties;
        }
    }
}
