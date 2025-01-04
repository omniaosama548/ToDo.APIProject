namespace ToDo.API.Error
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode,string?message=null)
        {
            StatusCode = statuscode;
            Message = message??GetDefaultMessageForStatusCode(StatusCode);
        }
        private string?GetDefaultMessageForStatusCode(int? statusCode)
        {
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "Un Autorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
