namespace ButlerLee.API.Models.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string errorCode)
            : base(System.Net.HttpStatusCode.NotFound, errorCode)
        {
        }
    }
}
