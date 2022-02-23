using Newtonsoft.Json;

namespace ButlerLee.API.Models.Exceptions
{
    public class GlobalException
    {
        /// <summary>
        /// 상태코드
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 에러코드
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 에러 메시지
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
