namespace ButlerLee.API.Models.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(string errorCode)
            : base(System.Net.HttpStatusCode.Conflict, errorCode)
        {
        }
    }
}
