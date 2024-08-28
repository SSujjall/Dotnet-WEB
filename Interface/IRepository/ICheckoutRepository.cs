namespace WEB.Interface.IRepository;

using WEB.DTOs;
using WEB.Models;

public interface ICheckoutRepository
{
    public Task<List<Checkout>> GetCheckout();
    public Task AddCheckout(Checkout checkoutModel);
    public Task<Checkout> ProcessCheckout(CheckoutDTO checkoutDto);
    public Task<Checkout> GetCheckoutById(Guid id);
    public Task SoftDeleteCheckout(Guid id);
}