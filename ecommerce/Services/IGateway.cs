using ecommerce.ViewModels;

namespace ecommerce.Services
{
    public interface IGateway
    {
        PaymentResult ProcessPayment(CheckoutViewModel model);
    }
}