using AutoMapper;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.IServices;
using eCommerce.BusinessLogicLayer.Mappers;
using eCommerce.DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.IRepository;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static Mysqlx.Expect.Open.Types;

namespace eCommerce.BusinessLogicLayer.Services
{
    public class ProductService : IProductServices
    {
        private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;

        private readonly IMapper _mapper;

        private readonly IProductRepository _productRepository;

        public ProductService(IValidator<ProductAddRequest> productAddRequestValidator, IValidator<ProductUpdateRequest> productUpdateRequestValidator, IMapper mapper, IProductRepository productRepository)
        {
            _productAddRequestValidator= productAddRequestValidator;
            _productUpdateRequestValidator= productUpdateRequestValidator;            
            _mapper = mapper;
            _productRepository= productRepository;
        }

        public async Task<ProductResponce?> AddProduct(ProductAddRequest request)
        {
            if(request==null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            //validate
            ValidationResult validation = await _productAddRequestValidator.ValidateAsync(request);
            if (!validation.IsValid) {
               string errors= string.Join(", ", validation.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);
            }

            // now mapping       ProductAddRequest to product
            Product productInput= _mapper.Map<Product>(request);
            //adding data

           Product?  Addedproduct= await _productRepository.AddProduct(productInput);

            if(Addedproduct==null) return null;

            // Now converting Product to ProductResponce so we can return data

            ProductResponce productResponce = _mapper.Map<ProductResponce>(Addedproduct);
            return productResponce;



        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            Product? exitingProduct= await _productRepository.GetProductByCondition(temp => temp.ProductID==id);
            if(exitingProduct==null) return false;

            //attempt to delete
            bool isdeleted= await _productRepository.DeleteProduct(id);
            return isdeleted;
        }

        public async Task<ProductResponce?> GetProductByCondition(Expression<Func<Product, bool>> condition)
        {
            Product? product= await _productRepository.GetProductByCondition(condition);
            if(product==null) return null;

            ProductResponce productResponce = _mapper.Map<ProductResponce>(product);
            return productResponce;
        }

        public async Task<List<ProductResponce?>> GetProductByConditions(Expression<Func<Product, bool>> condition)
        {
            IEnumerable<Product?> products = await _productRepository.GetProductsByConditions(condition);
            if (products == null) return null;

            IEnumerable<ProductResponce> productResponce = _mapper.Map<IEnumerable<ProductResponce>>(products);
            return productResponce.ToList();
        }

        public async Task<List<ProductResponce>> GetProducts()
        {
            IEnumerable<Product?> products = await _productRepository.GetProducts();
            if (products == null) return null;

            IEnumerable<ProductResponce?> productResponce = _mapper.Map<IEnumerable<ProductResponce>>(products);
          
            return productResponce.ToList();
        }

        public async Task<ProductResponce?> UpdateProduct(ProductUpdateRequest request)
        {
            Product? exitingProduct = await _productRepository.GetProductByCondition(temp => temp.ProductID == request.ProductID);

            if (exitingProduct == null) throw new ArgumentException("Invalid Product Id");


            // validators 

            ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);
            }

            //map data to ProductUpdateRequest to product

            Product productInput = _mapper.Map<Product>(request);

            Product? Updatedproduct= await _productRepository.UpdateProduct(productInput);

            ProductResponce UpdatedproductMap = _mapper.Map<ProductResponce>(Updatedproduct);

            return UpdatedproductMap;        

        }
    }
}
