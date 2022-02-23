using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ButlerLee.API.Models
{
    public class Card
    {
        [JsonProperty(PropertyName = "ccNumber")]
        public string CardNumber { get; set; }

        [JsonProperty(PropertyName = "ccNumberEndingDigits")]
        public int? CardEndingNumber { get; set; } = 0;

        [JsonProperty(PropertyName = "ccName")]
        public string CardUserName { get; set; }

        [JsonProperty(PropertyName = "ccExpirationMonth")]
        public string ExpirationMonth { get; set; }

        [JsonProperty(PropertyName = "ccExpirationYear")]
        public string ExpirationYear { get; set; }

        [JsonProperty(PropertyName = "cvc")]
        public string Cvc { get; set; }

        //public bool IsValidCreditCard()
        //{
        //    return this.GetType()
        //               .BaseType
        //               .GetProperties()
        //               .All(p => p.GetValue(this, null) != null);
        //}

        public bool IsCreditCardInfoValid()
        {
            if (this == null 
                || string.IsNullOrEmpty(CardNumber)
                || string.IsNullOrEmpty(CardUserName)
                || string.IsNullOrEmpty(ExpirationMonth)
                || string.IsNullOrEmpty(ExpirationYear)
                || string.IsNullOrEmpty(Cvc))
                return false;

            //new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
            var cardCheck = 
                new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^[0-9]{2}$");
            var cvcCheck = new Regex(@"^\d{3}$");
            var unspecialCharacterCheck = new Regex(@"[^a-zA-Z0-9가-힣\s]");

            string plainCardNumber = Regex.Replace(this.CardNumber, @"[^\d]", "");
            if (!cardCheck.IsMatch(plainCardNumber))
                return false;

            //특수문자 없을경우 true
            if (unspecialCharacterCheck.IsMatch(this.CardUserName))
                return false;

            if (!cvcCheck.IsMatch(this.Cvc))
                return false;

            if (!monthCheck.IsMatch(this.ExpirationMonth) || !yearCheck.IsMatch(this.ExpirationYear))
                return false;

            string firstTwoDigits = DateTime.UtcNow.Year.ToString().Substring(0, 2);
            var year = int.Parse($"{firstTwoDigits}{this.ExpirationYear}");
            var month = int.Parse(this.ExpirationMonth);

            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }
    }

}
