namespace Smart_Queue_API.DTOs
{
    public class ResultDto<T>
    {
        public bool IsSuccess {  get; set; }
        public string Message { get; set; }=string.Empty;
        public T? Data { get; set; }
        public static ResultDto<T> Success(T data, string message = "Operation was successfully performed!")
        {
            return new ResultDto<T> { IsSuccess = true, Data = data, Message = message };
        }
        public static ResultDto<T> Success(string message = "Operation was successfully performed!")
        {
            return new ResultDto<T> { IsSuccess = true, Message = message };
        }
        public static ResultDto<T> Failure(string message)
        {
            return new ResultDto<T> { IsSuccess = false, Message = message};
        }
    }
}
