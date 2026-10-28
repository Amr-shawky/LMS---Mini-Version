namespace LMS___Mini_Version.Mediators
{

    public class MediatorResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public static MediatorResult Fail(string message) => new()
        {
            IsSuccess = false,
            Message = message
        };

        public static MediatorResult Succeed(string message) => new()
        {
            IsSuccess = true,
            Message = message
        };
    }
}
