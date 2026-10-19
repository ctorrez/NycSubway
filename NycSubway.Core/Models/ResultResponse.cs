namespace NycSubway.Core.Models
{
    public class ResultResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ResultResponse SuccessResponse(string message = "Ok")
        {
            return new ResultResponse
            {
                Success = true,
                Message = message
            };
        }

        public static ResultResponse FailureResponse(string message)
        {
            return new ResultResponse
            {
                Success = false,
                Message = message
            };
        }
    }
}
