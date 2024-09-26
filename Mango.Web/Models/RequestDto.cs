using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string ApiUrl { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
