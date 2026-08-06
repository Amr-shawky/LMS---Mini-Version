namespace LMS___Mini_Version.Errors.Exeptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string? message = null)
            : base(message)
        {

        }
    }
}
