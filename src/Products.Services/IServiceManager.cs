namespace Products.Services;

public interface IServiceManager
{
    IProductService ProductService { get; }
}