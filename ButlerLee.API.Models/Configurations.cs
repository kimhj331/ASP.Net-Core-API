namespace ButlerLee.API.Models
{
    public class Configurations
    {
        public string ApplicationUrl { get; set; }
        public string ConnectionString { get; set; }
        public string UserId { get; set; }
        public SchedulerSettings SchedulerSettings { get; set; }
        public Configurations()
        {
            SchedulerSettings = new SchedulerSettings();
        }
    }
    public class SchedulerSettings
    { 
        //선점시간
        public uint PreemptingMinute { get; set; }

        //추가시간
        public uint ExtraMinute { get; set; }
    }

    public class HostAway
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Endpoint { get; set; }
        public string Scope { get; set; }
    }

    public class HostAwayClient
    {
        public string BaseAddress { get; set; }
        public string Timeout { get; set; }
        public string ExpiredTokenMessage { get; set; } 
    }

    public class KakaoPayClient
    {
        public string BaseAddress { get; set; }
        public string Timeout { get; set; }
        public string Schema { get; set; }
        public string Key { get; set; }
        public string Cid { get; set; }
    }

    public class TossPayClient
    {
        public string BaseAddress { get; set; }
        public string Timeout { get; set; }
        public string Schema { get; set; }
        public string Key { get; set; }
    }
}
