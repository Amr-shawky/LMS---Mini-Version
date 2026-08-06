namespace LMS___Mini_Version.Errors.Exeptions
{
    public class ValidationExcption : BadRequestException
    {
        public required IEnumerable<string> Errors { get; set; }

        public ValidationExcption(string message = "Bad Request")
            : base(message)
        {

        }
    }
}
