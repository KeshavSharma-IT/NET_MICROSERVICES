using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace eCommerce.BusinessLogicLayer.IServices
{
    public interface IProductServices
    {
        /// <summary>
        /// Retrieves the list of products from the products
        /// </summary>
        /// <returns></returns>
        Task<List<ProductResponce>> GetProducts();

        /// <summary>
        ///       Retrieves the list of products MATCHING WITH GIVEN CONDITION
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<ProductResponce?>> GetProductByConditions(Expression<Func<Product,bool>> condition);

        /// <summary>
        ///              Retrieves the  product MATCHING WITH GIVEN CONDITION
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<ProductResponce?> GetProductByCondition(Expression<Func<Product, bool>> condition);

                      /// <summary>
                      /// Add product and return responce or error if fails
                      /// </summary>
                      /// <param name="request"></param>
                      /// <returns></returns>

        Task<ProductResponce?> AddProduct(ProductAddRequest request);
        /// <summary>
        /// update product according to id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ProductResponce?> UpdateProduct(ProductUpdateRequest request);

        /// <summary>
        /// delete product according to id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteProduct(Guid id);


    }
}
