using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace ButlerLee.API.Models.Enumerations
{
    public enum ResponseStatus : short
    {
        [Description("성공")]
        [EnumMember(Value = "Success")]
        Success,

        [Description("실패")]
        [EnumMember(Value = "Fail")]
        Fail,
    }

    public enum Currency : short
    {
        [Description("원화")]
        [EnumMember(Value = "KRW")]
        KRW,

        [Description("달러")]
        [EnumMember(Value = "USD")]
        USD,
    }
    
    public enum CancelledBy : short
    {
        [Description("취소주체:호스트")]
        [EnumMember(Value ="Host")]
        Host,
        [Description("취소주체:게스트")]
        [EnumMember(Value = "Guest")]
        Guest,
    }

    public enum CalendarStatus : short
    {
        [Description("이용가능")]
        [EnumMember(Value = "Available")]
        Available,

        [Description("다수객실 보유시 모두 Block")]
        [EnumMember(Value = "MBlocked")]
        MBlocked,
        
        [Description("이용불가")]
        [EnumMember(Value = "Blocked")]
        Blocked,

        [Description("훗날 사용 위해 금지")]
        [EnumMember(Value = "HardBlock")]
        HardBlock,

        [Description("예약됨")]
        [EnumMember(Value = "Reserved")]
        Reserved,

        [Description("예약대기")]
        [EnumMember(Value = "Pending")]
        Pending,

        [Description("다수객실 보유시 모두 예약")]
        [EnumMember(Value = "MReserved")]
        MReserved
    }

    public enum ReservationStatus : short
    {
        [Description("New reservation, blocks calendar")]
        [EnumMember(Value = "New")]
        New,

        [Description("Reservation that has dates, guests, listing or pricing modified. Blocks calendar")]
        [EnumMember(Value = "Modified")]
        Modified,

        [Description("Reservation cancelled by either host or guest. Does not block calendar")]
        [EnumMember(Value = "Cancelled")]
        Cancelled,

        [Description("Hostaway specific status for reservations created by Owners that wish to block their properties usually because they plan to stay in them")]
        [EnumMember(Value = "OwnerStay")]
        OwnerStay,

        [Description("Airbnb only: for those clients using Airbnb’s Request to Book functionality. Client needs to approve or decline the reservation. If approved, the status will change to new. If declined, the status wil be expired")]
        [EnumMember(Value = "Pending")]
        Pending,

        [Description("Airbnb only: Intermediary reservation states that require guest action (no host action). If the guest fails to complete their tasks, this would result in status cancelled, otherwise status will be new. This status blocks the calendar")]
        [EnumMember(Value = "AwaitingPayment")]
        AwaitingPayment,

        [Description("Airbnb only as a result of declining a Request to Book reservation (pending)")]
        [EnumMember(Value = "Declined")]
        Declined,

        [Description("As explained in row 5 (pending)")]
        [EnumMember(Value = "Expired")]
        Expired,

        [Description("Vrbo only: similar to pending status for those clients that use Vrbo Request to Book functionality. Client needs to approve or decline the reservation. If approved the status will change to new, if declined it will change to cancelled")]
        [EnumMember(Value = "Unconfirmed")]
        Unconfirmed,

        [Description("Airbnb only: Intermediary reservation states that require guest action (no host action). If the guest fails to complete their tasks, this would result in status cancelled, otherwise status will be new. This status blocks the calendar")]
        [EnumMember(Value = "AwaitingGuestVerification")]
        AwaitingGuestVerification,

        [Description("Reservation status representing a guest question which doesn’t block the calendar")]
        [EnumMember(Value = "Inquiry")]
        Inquiry,

        [Description("Airbnb only: Hosts can preapprove the guest to encourage reservation. The host will have 24 hours to confirm their reservation. If they don’t the reservation will show status inquiryTimeout. The host can also decline the inquriy and the reservation will have status inquiryNotPossible.")]
        [EnumMember(Value = "InquiryPreapproved")]
        InquiryPreapproved,

        [Description("Airbnb only: If a host does not preapprove a guest they will receive a simple inquiry. Hosts will still have 24 to approve or deny de inquiry. If approved it will become a new reservation. If declined it will show status inquiryDenied")]
        [EnumMember(Value = "InquiryDenied")]
        InquiryDenied,

        [Description("as explained in row 13 (inquiryDenied)")]
        [EnumMember(Value = "InquiryTimedout")]
        InquiryTimedout,

        [Description("as explained in row 13 (inquiryDenied)")]
        [EnumMember(Value = "InquiryNotPossible")]
        InquiryNotPossible,

        [Description("Airbnb only: something made the inquiry fail.")]
        [EnumMember(Value = "Unknown")]
        Unknown,
    }

    public enum PaymentGateway : short
    {
        [Description("카카오페이")]
        [EnumMember(Value = "KakaoPay")]
        KakaoPay,

        [Description("토스페이")]
        [EnumMember(Value = "TossPay")]
        TossPay,

        [Description("직접카드결제")]
        [EnumMember(Value = "DirectCreditCard")]
        DirectCreditCard,
    }

    public enum PaymentType : short
    {
        [Description("카카오-카드결제")]
        [EnumMember(Value = "CARD")]
        Card,

        [Description("카카오-페이결제")]
        [EnumMember(Value = "MONEY")]
        Money
    }

    public enum KakaoStatus : short
    {

        [Description("결제 요청")]
        [EnumMember(Value = "READY")]
        Ready,

        [Description("결제 요청 메시지(TMS) 발송 완료")]
        [EnumMember(Value = "SEND_TMS")]
        SendTms,

        [Description("사용자가 카카오페이 결제 화면 진입")]
        [EnumMember(Value = "OPEN_PAYMENT")]
        OpenPayment,

        [Description("결제 수단 선택, 인증 완료")]
        [EnumMember(Value = "SELECT_METHOD")]
        SelectMethod,

        [Description("ARS 인증 진행 중")]
        [EnumMember(Value = "ARS_WAITING")]
        ArsWaiting,

        [Description("비밀번호 인증 완료")]
        [EnumMember(Value = "AUTH_PASSWORD")]
        AuthPassword,

        [Description("SID 발급 완료, 정기 결제 시 SID만 발급 한 경우")]
        [EnumMember(Value = "ISSUED_SID")]
        IssuedSid,

        [Description("결제 완료")]
        [EnumMember(Value = "SUCCESS_PAYMENT")]
        SuccessPayment,

        [Description("부분 취소")]
        [EnumMember(Value = "PART_CANCEL_PAYMENT")]
        PartCancelPayment,

        [Description("결제된 금액 모두 취소")]
        [EnumMember(Value = "CANCEL_PAYMENT")]
        CancelPayment,

        [Description("사용자 비밀번호 인증 실패")]
        [EnumMember(Value = "FAIL_AUTH_PASSWORD")]
        FailAuthPassword,

        [Description("사용자가 결제 중단")]
        [EnumMember(Value = "QUIT_PAYMENT")]
        QuitPayment,

        [Description("결제 승인 실패")]
        [EnumMember(Value = "FAIL_PAYMENT")]
        FailPayment,
    }

    public enum TossStatus : short
    {
        [Description("결제 요청")]
        [EnumMember(Value = "READY")]
        Ready,

        [Description("진행중")]
        [EnumMember(Value = "IN_PROGRESS")]
        InProgress,

        [Description("가상계좌 입금 대기 중")]
        [EnumMember(Value = "WAITING_FOR_DEPOSIT")]
        WaitingForDeposit,

        [Description("결제 완료됨")]
        [EnumMember(Value = "DONE")]
        Done,

        [Description("결제가 취소됨")]
        [EnumMember(Value = "CANCELED")]
        Canceled,

        [Description("결제가 부분 취소됨")]
        [EnumMember(Value = "PARTIAL_CANCELED")]
        PartialCanceled,

        [Description("결제가 부분 취소됨")]
        [EnumMember(Value = "Aborted")]
        Aborted,

        [Description("유효 시간(30분)이 지나 거래가 취소됨")]
        [EnumMember(Value = "EXPIRED")]
        Expired
    }
    

    public enum BookStatus : short
    {
        [Description("예약 불가")]
        Blocked,
        [Description("예약 가능")]
        Bookable,
        [Description("체크아웃만 가능")]
        CheckOutOnly,
        [Description("체크인만 가능")]
        CheckInOnly
    }

    public enum CallbackStatus : short
    {
        [EnumMember(Value = "Cancel")]
        Cancel,
        [EnumMember(Value = "Fail")]
        Fail
    }

    public enum CustomValueId : int
    {
        [EnumMember(Value = "eng_highlight")]
        EngHighlight = 42040,

        [EnumMember(Value = "kor_highlight")]
        Highlight = 42041,

        [EnumMember(Value = "eng_roomname")]
        EngName = 42039,

        [EnumMember(Value = "eng_description")]
        EngDescription = 42034,

        [EnumMember(Value = "Alt_location")]
        Latitude = 42232,
        
        [EnumMember(Value = "Long_location")]
        Longitude = 42233,

        [EnumMember(Value = "cleaning_fee")]
        CleaningFee = 42222,


    }
}
