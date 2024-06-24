namespace Application.Commons
{
    public class AppConfiguration
    {
        public string DatabaseConnection { get; set; }
        public JWTSection JWTSection { get; set; }
        public PayPalSettings PayPal { get; set; }
    }   
   
    public class JWTSection
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
    public class PayPalSettings
    {
        public string Client { get; set; }
        public string Secret { get; set; }
        public string mode { get; set; }
    }
}
