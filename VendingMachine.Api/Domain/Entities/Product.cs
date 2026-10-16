namespace VendingMachine.Api.Domain.Entities;

public class Product
{
    public string Code { get; }
    public string Name { get; }
    public string ProductType { get; }

    public Product(string code, string name, string productType)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Product code is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.");

        if (string.IsNullOrWhiteSpace(productType))
            throw new ArgumentException("Product type is required.");

        Code = code.Trim();
        Name = name.Trim();
        ProductType = productType.Trim();
    }
}