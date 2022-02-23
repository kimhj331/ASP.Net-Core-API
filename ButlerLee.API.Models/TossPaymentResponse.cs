using ButlerLee.API.Models.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerLee.API.Models
{
    public class TossPaymentResponse
    {
        /// <summary>
        /// 상점아이디
        /// </summary>
        public string MId { get; set; }
        /// <summary>
        /// Payment 객체의 응답 버전
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 결제 건에 대한 고유한 키 
        /// </summary>
        public string PaymentKey { get; set; }

        /// <summary>
        /// 가맹점에서 주문건에 대해 발급한 고유 ID
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 결제에 대한 주문명
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// 결제할 때 사용한 통화
        /// 원화인 KRW만 사용합니다.
        /// </summary>
        public string Currency { get; set; } = "KRW";

        /// <summary>
        /// 결제할 때 사용한 결제수단
        /// 카드, 가상계좌, 휴대폰, 계좌이체, 상품권(문화상품권, 도서문화상품권, 게임문화상품권) 중 하나입니다.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 총 결제금액
        /// </summary>
        public int? TotalAmount { get; set; }

        /// <summary>
        /// 취소할 수 있는 금액(잔고)
        /// </summary>
        public int? BalanceAmount { get; set; }

        /// <summary>
        /// 공급가액
        /// </summary>
        public int? SuppliedAmount { get; set; }

        /// <summary>
        /// 부가세
        /// </summary>
        public int? Vat { get; set; }

        /// <summary>
        /// 결제 처리 상태
        /// </summary>
        public TossStatus Status { get; set; }

        /// <summary>
        /// 결제요청이 일어난 날짜, 시간 정보
        /// </summary>
        public DateTime RequestedAt { get; set; }

        /// <summary>
        /// 결제승인이 일어난 날짜와 시간 정보
        /// </summary>
        public DateTime ApprovedAt { get; set; }

        /// <summary>
        /// 에스크로 사용 여부
        /// </summary>
        public bool UseEscrow { get; set; }

        /// <summary>
        /// 문화비 지출여부 
        /// </summary>
        public bool CultureExpense { get; set; }

        public TossCard Card { get; set; }

        /// <summary>
        /// 가상계좌 관련 정보
        /// </summary>
        //public object VirtualAccount { get; set; }

        /// <summary>
        /// 계좌이체 정보
        /// </summary>
        //public object Transfer { get; set; }

        /// <summary>
        /// 휴대폰 결제 관련 정보
        /// </summary>
        //public object MobilePhone { get; set; }

        /// <summary>
        ///  상품권 결제 관련 정보
        /// </summary>
        //public object GiftCertificate { get; set; }

        /// <summary>
        /// 현금 영수증 정보 
        /// </summary>
        //public object CashReceipt { get; set; }

        /// <summary>
        /// 카드사의 즉시 할인 프로모션 정보
        /// </summary>
        public int? DiscountAmount { get; set; }

        /// <summary>
        /// 결제취소 이력이 담기는 배열
        /// </summary>
        public IEnumerable<TossCancel> Cancels { get; set; }

        public TossCancel CancelData
        {
            get 
            {
                if (Cancels != null
                    && Cancels.Any())
                    return Cancels.First();

                return null;
            }
        }

        /// <summary>
        /// 가상계좌로 결제할 때 전달되는 입금 콜백을 검증하기 위한 값
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 결제 타입 정보
        /// NORMAL(일반결제), BILLING(자동결제), CONNECTPAY(커넥트페이) 중 하나
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 간편결제로 결제한 경우 간편결제 타입 정보
        /// </summary>
        public string EasyPay { get; set; }

        /// <summary>
        /// 면세금액
        /// </summary>
        public int? TaxFreeAmount { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string ErrorMessage { get; set; }
    }

    public class TossCard
    {
        /// <summary>
        /// 카드사 코드
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 카드번호
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 할부 개월 수
        /// </summary>
        public int InstallmentPlanMonths { get; set; }

        /// <summary>
        /// 무이자 할부 적용 여부
        /// </summary>
        public bool IsInterestFree { get; set; }

        /// <summary>
        /// 카드사 승인 번호
        /// </summary>
        public string ApproveNo { get; set; }

        /// <summary>
        /// 카드사 포인트 사용여부 
        /// </summary>
        public bool UseCardPoint { get; set; }

        /// <summary>
        /// 카드 종류
        /// 신용, 체크, 기프트 중 하나
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// 소유자 타입
        /// 개인, 법인 중 하나
        /// </summary>
        public string OwnerType { get; set; }

        /// <summary>
        /// 카드 결제의 매입 상태
        /// </summary>
        public string AcquireStatus { get; set; }

        /// <summary>
        /// 카드 매출전표 조회 페이지 주소
        /// </summary>
        public string ReceiptUrl { get; set; }
    }

    public class TossCancel
    { 
        /// <summary>
        /// 결제 건에 대한 고유한 키값
        /// </summary>
        public string PaymentKey { get; set; }
        /// <summary>
        /// 결제를 취소하는 이유
        /// </summary>
        public string CancelReason { get; set; }

        /// <summary>
        /// 취소할 금액
        /// </summary>
        public int? CancelAmount { get; set; }

        /// <summary>
        /// 면세 금액
        /// </summary>
        public int? TaxFreeAmount { get; set; }

        public DateTime? CanceledAt { get; set; }
    }
}
