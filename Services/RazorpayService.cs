using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace RazorpayRouteDemo.Services
{
    public class RazorpayService
    {
        private readonly string _key;
        private readonly string _secret;

        public RazorpayService(IConfiguration config)
        {
            _key = config["Razorpay:Key"];
            _secret = config["Razorpay:Secret"];

            if (string.IsNullOrEmpty(_key) || string.IsNullOrEmpty(_secret))
                throw new Exception("Razorpay keys missing");
        }

        // ✅ ADD THIS METHOD (YOUR ERROR FIX)
        public string GetKey()
        {
            return _key;
        }

        private RazorpayClient GetClient()
        {
            return new RazorpayClient(_key, _secret);
        }

        // ✅ CREATE ORDER
        public Order CreateOrder(int amount)
        {
            if (amount < 1)
                throw new Exception("Amount must be at least ₹1");

            var client = GetClient();

            var options = new Dictionary<string, object>
            {
                { "amount", amount * 100 },
                { "currency", "INR" },
                { "payment_capture", 1 }
            };

            var order = client.Order.Create(options);

            Console.WriteLine("Razorpay Amount: " + order["amount"]);

            return order;
        }


        // ✅ VERIFY PAYMENT
        public bool VerifyPayment(string orderId, string paymentId, string signature)
        {
            var payload = orderId + "|" + paymentId;

            var keyBytes = Encoding.UTF8.GetBytes(_secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(payloadBytes);

            var generatedSignature = BitConverter.ToString(hash)
                .Replace("-", "")
                .ToLower();

            return generatedSignature == signature;
        }
    }
}