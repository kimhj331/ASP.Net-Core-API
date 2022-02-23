using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Filters;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ButlerLee.API.Models
{
    public class KakaoPaymentResponse : KakaoPayErrorResponse
    {
        public PaymentGateway PaymentAgency { get; set; }
        /// <summary>
        /// 예약번호
        /// </summary>
        public int? ReservationId { get; set; }
        /// <summary>
        /// 고객 제공 예약 ID
        /// </summary>
        public string ReservationNo { get; set; }

        /// <summary>
        /// 요청 고유 번호
        /// </summary>
        [JsonProperty(PropertyName = "aid")]
        public string Aid { get; set; }

        /// <summary>
        /// 결제 고유 번호
        /// </summary>
        [JsonProperty(PropertyName = "tid")]
        public string Tid { get; set; }


        /// <summary>
        /// 가맹점 코드
        /// </summary>
        [JsonProperty(PropertyName = "cid")]
        public string Cid { get; set; }

        /// <summary>
        /// 가맹점 코드 인증키
        /// </summary>
        [JsonProperty(PropertyName = "cid_secret")]
        public string CidSecret { get; set; }

        /// <summary>
        /// 정기 결제용 ID, 정기 결제 CID로 단건 결제 요청시 발급
        /// </summary>
        [JsonProperty(PropertyName = "sid")]
        public string Sid { get; set; }
        
        /// <summary>
        /// 결제 승인 요청을 인증하는 토큰
        /// </summary>
        [JsonProperty(PropertyName = "pg_token")]
        public string PgToken { get; set; }

        /// <summary>
        /// 결제상태
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 가맹점 주문번호
        /// </summary>
        [JsonProperty(PropertyName = "partner_order_id")]
        public string PartnerOrderId { get; set; }

        /// <summary>
        /// 가맹점 회원 id
        /// </summary>
        [JsonProperty(PropertyName = "partner_user_id")]
        public string PartnerUserId { get; set; }

        /// <summary>
        /// 상품이름
        /// </summary>
        [JsonProperty(PropertyName = "item_name")]
        public string ItemName { get; set; }

        /// <summary>
        /// 상품코드
        /// </summary>
        [JsonProperty(PropertyName = "item_code")]
        public string ItemCode { get; set; }

        /// <summary>
        /// 상품수량
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; set; }

        /// <summary>
        /// 결제수단,  CARD 또는 MONEY 중 하나
        /// </summary>
        [JsonProperty(PropertyName = "payment_method_type")]
        public PaymentType? PaymentMethodType { get; set; }

        /// <summary>
        /// 결제 준비 요청시각
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 결제 승인 시각
        /// </summary>
        [JsonProperty(PropertyName = "approved_at")]
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// 결제 승인 요청에 대해 저장하고 싶은 값
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// 결제 업데이트 시각
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        [JsonProperty(PropertyName = "canceled_at")]
        public DateTime? CanceledAt { get; set; }

        /// <summary>
        /// 결제금액정보
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public Amount Amount { get; set; }

        /// <summary>
        /// 결제 카드정보
        /// </summary>
        [JsonProperty(PropertyName = "card_info")]
        public CardInfo CardInfo { get; set; }

        /// <summary>
        /// 취소 정보
        /// </summary>
        [JsonProperty(PropertyName = "canceled_amount")]
        public CanceledAmount CanceledAmount { get; set; }

        public KakaoPaymentResponse()
        {
            Amount = new Amount();
            CardInfo = new CardInfo();
            CanceledAmount = new CanceledAmount();
        }
    }

    #region Amount
    public class Amount
    {
        public int? Id { get; set; }
        public int? PaymentId { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int? Total { get; set; }

        [JsonProperty(PropertyName = "tax_free")]
        public int? TaxFree { get; set; }

        [JsonProperty(PropertyName = "vat")]
        public int? Vat { get; set; }
        public int? Point { get; set; }
        public int? Discount { get; set; }
    }
    #endregion

    #region CardInfo
    public class CardInfo
    {
        [JsonProperty(PropertyName = "purchase_corp")]
        public string PurchaseCorp { get; set; }

        [JsonProperty(PropertyName = "purchase_corp_code")]
        public string PurchaseCorpCode { get; set; }

        [JsonProperty(PropertyName = "issuer_corp")]
        public string IssuerCorp { get; set; }

        [JsonProperty(PropertyName = "issuer_corp_code")]
        public string IssuerCorpCode { get; set; }

        [JsonProperty(PropertyName = "kakaopay_purchase_corp")]
        public string KakaopayPurchaseCorp { get; set; }

        [JsonProperty(PropertyName = "kakaopay_purchase_corp_code")]
        public string KakaopayPurchaseCorpCode { get; set; }

        [JsonProperty(PropertyName = "kakaopay_issuer_corp")]
        public string KakaopayIssuerCorp { get; set; }

        [JsonProperty(PropertyName = "kakaopay_issuer_corp_code")]
        public string KakaopayIssuerCorpCode { get; set; }

        /// <summary>
        /// 카드 앞 6자리
        /// </summary>
        [JsonProperty(PropertyName = "bin")]
        public string Bin { get; set; }


        /// <summary>
        /// 카드타입
        /// </summary>
        [JsonProperty(PropertyName = "card_type")]
        public string CardType { get; set; }

        /// <summary>
        /// 할부개월
        /// </summary>
        [JsonProperty(PropertyName = "install_month")]
        public string InstallMonth { get; set; }

        /// <summary>
        /// 승인번호
        /// </summary>
        [JsonProperty(PropertyName = "approved_id")]
        public string ApprovedId { get; set; }

        [JsonProperty(PropertyName = "card_mid")]
        public string CardMid { get; set; }

        /// <summary>
        /// 할부사용여부 
        /// "N" : 미사용
        /// "Y" : 사용
        /// </summary>
        [JsonProperty(PropertyName = "interest_free_install")]
        public string InterestFreeInstall { get; set; }
        
        [JsonProperty(PropertyName = "card_item_code")]
        public string CardItemCode { get; set; }
    }
    #endregion

    #region CanceledAmount
    public class CanceledAmount
    {
        
        public int? Total { get; set; }
        public uint? TaxFree { get; set; }
        public uint? Vat { get; set; }
        public uint? Point { get; set; }
        public int? Discount { get; set; }
    }
    #endregion

    public class KakaoCanelRequest : KakaoPaymentResponse
    {
        /// <summary>
        /// 취소금액
        /// </summary>
        [JsonProperty(PropertyName = "cancel_amount")]
        public int CancelAmount { get; set; }

        [JsonProperty(PropertyName = "cancel_tax_free_amount")]
        public int CancelTaxFreeAmount { get; set; }
    }
}
