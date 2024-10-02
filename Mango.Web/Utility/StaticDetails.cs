namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        public static string CouponAPIBaseURL { get; set; }
        public static string AuthAPIBaseURL { get; set; }
        public static string ProductAPIBaseURL { get; set; }
        public static string ShoppingCartAPIBaseURL { get; set; }

        public const string TokenCookie = "JwtToken";
        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
    
}
