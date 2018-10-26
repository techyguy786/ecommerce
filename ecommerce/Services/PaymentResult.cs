namespace ecommerce.Services
{
    public class PaymentResult
    {
        public PaymentResult(string transactionId, bool success, string message)
        {
            TransactionId = transactionId;
            Succeeded = success;
            Message = message;
        }

        public string TransactionId { get; }
        public bool Succeeded { get; }
        public string Message { get; }

    }
}