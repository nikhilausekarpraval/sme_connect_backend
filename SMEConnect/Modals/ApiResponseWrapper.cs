using System.Text.Json.Serialization;

namespace SMEConnect.Modals
{
    public class ApiResponseWrapper<T>
    {
        [JsonPropertyName("value")]
        public T? Value { get; set; }
    }
}
