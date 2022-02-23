namespace ButlerLee.API.Models.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string errorCode)
            : base(System.Net.HttpStatusCode.BadRequest, errorCode)
        {
        }
    }
}
