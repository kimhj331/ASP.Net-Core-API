using System;

namespace ButlerLee.API.HttpClient.Options
{
    /// <summary>
    /// 일정 수의 요청이 실패가 될 경우, 일정시간 동안 새 요청 수행 중지
    /// 클라이언트는 시간 초과를 기다리지 않고 실패 응답을 빠르게 받음.
    /// </summary>
    public class CircuitBreakerPolicyOptions
    {
        /// <summary>
        /// 중지 기간
        /// </summary>
        public TimeSpan DurationOfBreak { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 요청 실패 수
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 12;
    }
}
