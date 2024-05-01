namespace Services.HandleReponses
{
    public class APIResponse
    {
        public APIResponse(int statusCode, string message = null) {

            StatusCode = statusCode;
            StatusMessage = message ?? GetDefaultMessageForStatusCode(statusCode);
        }    

        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        private string GetDefaultMessageForStatusCode(int code)
        {
            // match pattern 
            return code switch
            {
                400 => "Bad Request",
                401 => "You are not Authorized",
                404 => "Resource not found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
