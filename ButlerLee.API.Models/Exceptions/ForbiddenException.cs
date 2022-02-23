namespace ButlerLee.API.Models.Exceptions
{
    public class ForbiddenException : CustomException
    {
        public ForbiddenException(string errorCode)
            : base(System.Net.HttpStatusCode.Forbidden, errorCode)
        {
        }
    }
}
