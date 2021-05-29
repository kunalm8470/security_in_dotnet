using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class HttpError
    {
        [JsonPropertyName("error_code")]
        public string ErrorCode { get; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; }

        public HttpError(string errorCode, string errorDescription)
        {
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }
    }
}
