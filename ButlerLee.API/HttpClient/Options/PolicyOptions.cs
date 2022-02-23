namespace ButlerLee.API.HttpClient.Options
{
    /// <summary>
    /// 정책 옵션
    /// </summary>
    public class PolicyOptions
    {
        /// <summary>
        /// 일시적 중지 정책
        /// </summary>
        public CircuitBreakerPolicyOptions HttpCircuitBreaker { get; set; }

        /// <summary>
        /// 재시도 정책
        /// </summary>
        public RetryPolicyOptions HttpRetry { get; set; }
    }
}
