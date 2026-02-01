
namespace eCommerce.BusinessLogicLayer.DTO
{
    public record ProductResponce(Guid ProductID, string ProductName, CategoryOptions Category, double? UnitPrice, int? QuantityInStock)
    {
        public ProductResponce() : this(default, default, default, default, default) { }

    }
}
