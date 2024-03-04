using Products.Model;

namespace Products.Services.Results;

public record UpdateProductResult(bool Success, Product? Product, ResultErrorReason ErrorReason, IDictionary<string, string> Errors)
{
    public static UpdateProductResult FromSuccess(Product product) =>
        new(true, product, ResultErrorReason.None, new Dictionary<string, string>(0));

    public static UpdateProductResult FromNotFoundError() =>
        new(false, null, ResultErrorReason.NotFound, new Dictionary<string, string>(0));

    public static UpdateProductResult FromValidationError(IDictionary<string, string> errors) => 
        new(false, null, ResultErrorReason.Validation, errors);
}
