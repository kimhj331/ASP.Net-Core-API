namespace ButlerLee.API.HttpClient.Options
{
    /// <summary>
    /// 재시도 정책 옵션
    /// </summary>
    public class RetryPolicyOptions
    {
        /// <summary>
        /// 재시도 횟수
        /// </summary>
        public int Count { get; set; } = 3;

        /// <summary>
        /// 지수적 백오프 (지연간격, second)
        /// </summary>
        public int BackoffPower { get; set; } = 2;
    }
}
