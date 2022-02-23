using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ButlerLee.API.Models
{
    public class KakaoPayReadyRequest : BaseParameters //: KakaoPaymentResponse
    {
        [JsonProperty(PropertyName = "cid")]
        public string Cid { get; set; }

        [JsonProperty(PropertyName = "partner_order_id")]
        public string PartnerOrderId { get; set; }

        [JsonProperty(PropertyName = "partner_user_id")]
        public string PartnerUserId { get; set; }

        [JsonProperty(PropertyName = "item_name")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "item_code")]
        public string ItemCode { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "total_amount")]
        public int TotalAmount { get; set; }

        [JsonProperty(PropertyName = "tax_free_amount")]
        public int TaxFreeAmount { get; set; }

        [JsonProperty(PropertyName = "vat_amount")]
        public int? VatAmount { get; set; }

        /// <summary>
        /// 결제 성공 시 redirect url
        /// </summary>
        [JsonProperty(PropertyName = "approval_url")]
        public string ApprovalUrl { get; set; }

        /// <summary>
        /// 결제 취소 시 redirect url
        /// </summary>
        [JsonProperty(PropertyName = "cancel_url")]
        public string CancelUrl { get; set; }

        /// <summary>
        /// 결제 실패 시 redirect url
        /// </summary>
        [JsonProperty(PropertyName = "fail_url")]
        public string FailUrl { get; set; }

        /// <summary>
        /// 카드 할부개월, 0~12
        /// </summary>
        [JsonProperty(PropertyName = "install_month")]
        public int? InstallMonth { get; set; }

        public int ReservationId { get; set; }

        public PaymentType PaymentMethodType {get;set;}
    }

    public class KakaoPayReadyResponse : KakaoPayErrorResponse
    {
        /// <summary>
        /// 결제 고유 번호
        /// </summary>
        [JsonProperty(PropertyName = "tid")]
        public string Tid { get; set; }

        /// <summary>
        /// 요청한 클라이언트가 모바일 앱인경우 
        /// </summary>
        //[JsonProperty(PropertyName = "next_redirect_app_url")]
        //private string NextRedirectAppUrl { get; set; }

        /// <summary>
        /// 요청한 클라이언트가 모바일 웹일 경우
        /// </summary>
        [JsonProperty(PropertyName = "next_redirect_mobile_url")]
        public string NextRedirectMobileUrl { get; set; }

        /// <summary>
        /// 요청한 클라이언트가 PC 웹일 경우
        /// 카카오톡으로 결제 요청 메시지(TMS)를 보내기 위한 사용자 정보 입력 화면 Redirect URL
        /// </summary>
        [JsonProperty(PropertyName = "next_redirect_pc_url")]
        public string NextRedirectPcUrl { get; set; }

        /// <summary>
        /// 카카오페이 결제 화면으로 이동하는 Android 앱 스킴(Scheme)
        /// </summary>
        //[JsonProperty(PropertyName = "android_app_scheme")]
        //private string AndroidAppScheme { get; set; }

        /// <summary>
        /// 카카오페이 결제 화면으로 이동하는 iOS 앱 스킴
        /// </summary>
        //[JsonProperty(PropertyName = "ios_app_scheme")]
        //private string IosAppScheme { get; set; }

        /// <summary>
        /// 결제 준비 요청 시간
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        public KakaoPayReadyResponse() { }
    }

   
}
