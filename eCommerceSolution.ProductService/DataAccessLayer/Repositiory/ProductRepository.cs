using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Repositiory
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task<Product?> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(temp => temp.ProductID == id);
            if (product == null) 
            {
                return false;
            }

            _context.Products.Remove(product);
            int affectedRow =await _context.SaveChangesAsync();
            return affectedRow > 0;

        }

        public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            return await _context.Products.FirstOrDefaultAsync(conditionExpression);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product?>> GetProductsByConditions(Expression<Func<Product, bool>> conditionExpression)
        {
            return await _context.Products.Where(conditionExpression).ToListAsync();
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            Product? existingproduct = await _context.Products.FirstOrDefaultAsync(temp => temp.ProductID == product.ProductID);
            if (existingproduct == null)
            {
                return null;
            }

            
            existingproduct.ProductName = product.ProductName;
            existingproduct.UnitPrice = product.UnitPrice;
            existingproduct.QuantityInStock = product.QuantityInStock;

            await _context.SaveChangesAsync();
            return existingproduct;

        }
    }
}
