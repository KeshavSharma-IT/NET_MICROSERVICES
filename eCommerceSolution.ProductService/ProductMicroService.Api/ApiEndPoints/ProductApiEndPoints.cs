using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.IServices;
using FluentValidation;
using FluentValidation.Results;

namespace eCommerce.ProductMicroService.Api.ApiEndPoints
{
    public static class ProductApiEndPoints
    {
        public static IEndpointRouteBuilder MapProductApiEndPoints(this IEndpointRouteBuilder app)
        {

            //Get   api/products


            app.MapGet("/api/products", async (IProductServices productServices) =>
            {
               List<ProductResponce> productResponces= await productServices.GetProducts();

                return Results.Ok(productResponces);

            });

            //Get   api/products/search/productId/12
            app.MapGet("/api/products/search/productId/{ProductID:Guid}", async (IProductServices productServices,Guid ProductID) =>
            {
                ProductResponce? productResponces = await productServices.GetProductByCondition(temp=>temp.ProductID == ProductID);

                return Results.Ok(productResponces);

            });

            //Get   api/products/search/name or cateogory
            app.MapGet("/api/products/search/{searchString}", async (IProductServices productServices, string searchString) =>
            {
                List<ProductResponce> productsByProductName = await productServices.GetProductByConditions(temp => temp.ProductName!=null && temp.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                List<ProductResponce> productsByCategory = await productServices.GetProductByConditions(temp => temp.Category!=null && temp.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase));

                var products = productsByProductName.Union(productsByCategory);
                return Results.Ok(products);

            });

            //Post   api/products/
            app.MapPost("/api/products", async (IProductServices productServices, IValidator<ProductAddRequest> validator ,ProductAddRequest productAddRequest) =>
            {

                // Validate the ProductAddReques object using Fluent validation

                ValidationResult validationResult= await validator.ValidateAsync(productAddRequest);

                if(!validationResult.IsValid)
                {
                 Dictionary<string,string[]> errors= validationResult.Errors
                    .GroupBy(temp=>temp.PropertyName)
                    .ToDictionary(grp=>grp.Key,
                    grp=>grp.Select(err=>err.ErrorMessage).ToArray());
                    //throw new ArgumentException(errors);
                    return Results.ValidationProblem(errors);
                }

             var addedProductResponce=   await productServices.AddProduct(productAddRequest);
                if (addedProductResponce != null)
                {
                    return Results.Created($"/api/products/search/productId/{addedProductResponce.ProductID}", addedProductResponce);
                }
                else
                {
                    return Results.Problem("Errors in Adding");
                }

            });


            //Put   api/products/
            app.MapPut("/api/products", async (IProductServices productServices, IValidator<ProductUpdateRequest> validator, ProductUpdateRequest productUpdateRequest) =>
            {

                // Validate the ProductAddReques object using Fluent validation

                ValidationResult validationResult = await validator.ValidateAsync(productUpdateRequest);

                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors
                       .GroupBy(temp => temp.PropertyName)
                       .ToDictionary(grp => grp.Key,
                       grp => grp.Select(err => err.ErrorMessage).ToArray());
                    //throw new ArgumentException(errors);
                    return Results.ValidationProblem(errors);
                }

                var UpdatedProductResponce = await productServices.UpdateProduct(productUpdateRequest);
                if (UpdatedProductResponce != null)
                {
                    return Results.Created($"/api/products/search/productId/{UpdatedProductResponce.ProductID}", UpdatedProductResponce);
                }
                else
                {
                    return Results.Problem("Errors in Adding");
                }

            });

            //Delete   api/products/
            app.MapDelete("/api/products/{ProductID:Guid}", async (IProductServices productServices,Guid ProductID) =>
            {

                bool isDeleted = await productServices.DeleteProduct(ProductID);
                if (isDeleted)
                {
                    return Results.Ok(true);
                }
                else
                {
                    return Results.Problem("Errors in Deleting");
                }

            });



            return app;
        }
    }
}
