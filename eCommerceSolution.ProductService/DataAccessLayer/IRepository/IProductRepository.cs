

using eCommerce.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace eCommerce.DataAccessLayer.IRepository
{
    public interface IProductRepository
    {
        /// <summary>
        /// retrives all products  from table
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProducts();

        /// <summary>
        ///    get products based on the specified condition
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<IEnumerable<Product?>> GetProductsByConditions(Expression<Func<Product, bool>> conditionExpression);


        /// <summary>
        ///    get single product based on specified condition
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);

        
        /// <summary>
        /// add new product into the product table
        /// </summary>
        /// <param name="product"></param>
        /// <returns>     returns the added products objects or null if unsuccessful
        /// </returns>
        Task<Product?> AddProduct(Product product);

        /// <summary>
        ///  Update an exiting products
        /// </summary>
        /// <param name="product"></param>
        /// <returns> return the upated product or null if not found</returns>

        Task<Product?> UpdateProduct(Product product);


        /// <summary>
        /// delete product according to ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteProduct(Guid id);

    }
}
