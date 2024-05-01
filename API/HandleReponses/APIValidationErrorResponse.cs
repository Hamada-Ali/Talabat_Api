namespace API.HandleReponses
{
    public class APIValidationErrorResponse : APIException
    {
        public APIValidationErrorResponse() : base(400)
        {

        }
        public IEnumerable<string> Errors { get; set; }
    }
}
