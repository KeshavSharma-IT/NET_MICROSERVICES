using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.IServices;
using DataAccessLayer.Entities;
using DataAccessLayer.IRepository;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLogicLayer.Services
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
            if (validation.IsValid) {
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
            throw new NotImplementedException();
        }

        public async Task<ProductResponce?> GetProductByCondition(Expression<Func<Product, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductResponce?>> GetProductByConditions(Expression<Func<Product, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductResponce>> GetProducts()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponce?> UpdateProduct(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
