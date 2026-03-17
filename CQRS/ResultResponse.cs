namespace LMS___Mini_Version.CQRS
{
    public class ResultResponse<T> 
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public static ResultResponse<T> Success(T data,string message = null)
        {
            return new ResultResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        } 
        public static ResultResponse<T> Faild(string message, List<string> errors=null)
        {
            return new ResultResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors= errors
            };
        } 
    }
}
