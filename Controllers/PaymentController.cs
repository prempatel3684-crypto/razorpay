using Microsoft.AspNetCore.Mvc;
using RazorpayRouteDemo.Models;
using RazorpayRouteDemo.Services;

namespace RazorpayRouteDemo.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly RazorpayService _service;

        public PaymentController(RazorpayService service)
        {
            _service = service;
        }

        // 🔐 send key safely to frontend
        [HttpGet("get-key")]
        public IActionResult GetKey()
        {
            return Ok(new { key = _service.GetKey() });
        }

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] PaymentRequest request)
        {
            Console.WriteLine("Amount Received: " + request.Amount);

            var order = _service.CreateOrder(request.Amount);

            // ✅ FIX: RETURN CLEAN DATA
            return Ok(new
            {
                id = order["id"].ToString(),
                amount = Convert.ToInt32(order["amount"]),
                currency = order["currency"].ToString()
            });
        }


        // ✅ VERIFY PAYMENT
        [HttpPost("verify-payment")]
        public IActionResult VerifyPayment([FromBody] VerifyPaymentRequest request)
        {
            var isValid = _service.VerifyPayment(
                request.razorpay_order_id,
                request.razorpay_payment_id,
                request.razorpay_signature
            );

            return isValid
                ? Ok("Payment verified ✅")
                : BadRequest("Payment failed ❌");
        }
    }
}