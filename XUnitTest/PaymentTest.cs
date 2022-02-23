using AutoMapper;
using ButlerLee.API.Entities;
using ButlerLee.API.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace XUnitTest
{
    public class PaymentTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IMapper _mapper;
        
        public PaymentTest(CustomWebApplicationFactory factory)
        {
            _mapper = factory.Mapper;
        }

        [Fact]
        public void GetPayments()
        {
            ButlerLee.API.Models.KakaoPaymentResponse payment = new ButlerLee.API.Models.KakaoPaymentResponse();
            

            //var result = _mapper.Map<Payment>(payment);

            //Assert.True(result != null);
        }

        [Fact]
        public void ToSnakeCase()
        {
            ButlerLee.API.Models.KakaoPaymentResponse payment = new ButlerLee.API.Models.KakaoPaymentResponse();
            payment.Amount.Total = 12345;
            payment.CidSecret = "12346asd";

            //var teststr = payment.GetQueryString();
            //Assert.True(teststr != null);
        }

        [Fact]
        public void GenerateReservationNo()
        {
            Guid guid = Guid.NewGuid();
            string reservationNo = Convert.ToBase64String(guid.ToByteArray());
            reservationNo = reservationNo.Replace("=", ""); //remove '='

            Assert.True(string.IsNullOrEmpty(reservationNo));
        }

        [Fact]
        public void GenerateReservationNo2()
        {
            const string src = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            int length = 6;

            var sb = new StringBuilder();
            Random RNG = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[RNG.Next(0, src.Length)];
                sb.Append(c);
            }

            string reservationNo = sb.ToString();
            Assert.True(string.IsNullOrEmpty(reservationNo) == false);
        }

        public void ToBase64()
        { 
          
        }

    }
    public static class TestClass
    {
        public static string GetSnakeQueryString(this object testobj, string separator = ",")
        {
            var properties = testobj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetValue(testobj, null) != null)
                .ToDictionary(x => x.GetCustomAttribute<JsonPropertyAttribute>() == null ? x.Name :
                x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName, x => x.GetValue(testobj, null));
          

            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

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
                        //properties[key]
                    }

                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }
             
            return string.Join("&", properties
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key.ToSnakeCase()), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));

        }
    }

   
}
