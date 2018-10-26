using Braintree;
using ecommerce.ViewModels;

namespace ecommerce.Services
{
    public class PaymentGateway : IGateway
    {
        /*These API keys have been disabled. Always keep API keys private! Never share them with others or commit them to a public GitHub repository.*/
        private BraintreeGateway _gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "ndp68r72vxv4dgmn",
            PublicKey = "mg3kx8k78ybbwy5h",
            PrivateKey = "e6ac778a3a0033eb03c02a03a726bb12"
        };

        public PaymentResult ProcessPayment(CheckoutViewModel model)
        {
            var request = new TransactionRequest()
            {
                Amount = model.Sum,
                CreditCard = new TransactionCreditCardRequest()
                {
                    Number = model.CardNumber,
                    CVV = model.Cvv,
                    ExpirationMonth = model.Month,
                    ExpirationYear = model.Year
                },
                Options = new TransactionOptionsRequest()
                {
                    SubmitForSettlement = true
                }
            };

            var result = _gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                return new PaymentResult(result.Target.Id, true, "Transaction Complete");
            }

            return new PaymentResult(null, false, result.Message);
        }
    }
}