using BrandLook.Entities;

namespace BandLookMVC.Repositories;

public interface IProductRepository
{
    public Task<IEnumerable<Product>> ListProduct(int top);
    
}