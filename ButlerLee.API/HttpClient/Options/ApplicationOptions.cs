namespace ButlerLee.API.HttpClient.Options
{
    public class ApplicationOptions
    {
        public PolicyOptions Policies { get; set; }

        public HostAwayClientOptions HostAwayClient { get; set; }

        public KakaoPayClientOptions KakaoPayClient { get; set; }

        public TossPayClientOptions TossPayClient { get; set; }
    }
}
