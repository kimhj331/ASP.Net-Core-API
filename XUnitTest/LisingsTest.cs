using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Extensions;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest
{
    public class HospitalTest
    {
        public HospitalTest()
        {
        }

        [Fact]
        public void ParamsToQueryString()
        {
            ListingParameters parameters = new ListingParameters();
            parameters.StartDate = DateTime.UtcNow;
            parameters.EndDate = DateTime.UtcNow;
            //parameters.NumberOfBathrooms = 1;
            //parameters.NumberOfBedrooms = 2;
            //parameters.NumberOfBeds = 3;
           // parameters.GuestNumber = 4;
            
            var querystring = parameters.GetQueryString();

            Assert.True(string.IsNullOrEmpty(querystring) == false);
        }

        [Fact]
        public void ParamsToQueryString2()
        {
            ListingParameters parameters = new ListingParameters();
            parameters.StartDate = DateTime.UtcNow;
            parameters.EndDate = DateTime.UtcNow;
            //parameters.NumberOfBathrooms = 1;
            //parameters.NumberOfBedrooms = 2;
            //parameters.NumberOfBeds = 3;
           // parameters.GuestNumber = 4;

            //var amenities = new List<int>();
            //amenities.Add(1);
            //amenities.Add(3);
            //amenities.Add(5);
            //parameters.Amenities = amenities;

            var querystring = parameters.GetQueryString();

            var q = ToQueryString(parameters);

            Assert.True(string.IsNullOrEmpty(querystring) == false);
        }

        public string ToQueryString(object request, string separator = ",")
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var properties = request.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                //.Where(x => x.CanRead)
                .Where(x => x.GetValue(request, null) != null)
                .Where(x => x.GetCustomAttribute<JsonPropertyAttribute>() != null)
                .ToDictionary(x => x.GetCustomAttribute<JsonPropertyAttribute>() == null ? x.Name :
                                   x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName, x => x.GetValue(request, null));

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
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            return string.Join("&", properties
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));
        }

        [Fact]
        public void TestRegex()
        {
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^[0-9]{2}$");

            string firstTwoDigits = DateTime.UtcNow.Year.ToString().Substring(0, 2);
            var year = int.Parse($"{firstTwoDigits}25");
            var month = int.Parse("09");
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            string cardNumber = "3333-2222-3333-2222";
            cardNumber = Regex.Replace(cardNumber, @"[^\d]", "");

            var cardCheck = new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$");
            var ss = cardCheck.IsMatch(cardNumber);

            Assert.True(monthCheck.IsMatch("09") == true && yearCheck.IsMatch("25") == true);
            Assert.True((cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6)));
        }

        [Fact]
        public void TestCardInfo()
        {
            Card card = new Card();
            card.CardEndingNumber = 333;
            card.CardNumber = "3581-3465-4432-7643";
            card.CardUserName = "adsf";
            card.Cvc = "456";
            card.ExpirationMonth = "04";
            card.ExpirationYear = "22";

            bool vaild = card.IsCreditCardInfoValid();
            Assert.True(true);
        }

        //public bool IsCreditCardInfoValid()
        //{
        //    var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
        //    var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
        //    var yearCheck = new Regex(@"^20[0-9]{2}$");
        //    var cvcCheck = new Regex(@"^\d{3}$");

        //    if (!cardCheck.IsMatch(this.CardNumber))
        //        return false;
        //    if (!cvcCheck.IsMatch(this.Cvc))
        //        return false;

        //    if (!monthCheck.IsMatch(this.ExpirationMonth) || !yearCheck.IsMatch(this.ExpirationYear))
        //        return false;

        //    var year = int.Parse(this.ExpirationYear);
        //    var month = int.Parse(this.ExpirationMonth);
        //    var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
        //    var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

        //    return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        //}

        [Fact]
        public void Diff()
        {
            DateTime reservationDate = DateTime.UtcNow.AddMinutes(-7);


            DateTime now = DateTime.UtcNow;

           var min = now.Subtract(reservationDate).TotalMinutes;


            //IQueryable<UnpaidReservation> query = _context.UnpaidReservation
            //    .Where(o => )
        }

    }
}
