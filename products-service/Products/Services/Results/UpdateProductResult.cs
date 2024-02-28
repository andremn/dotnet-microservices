namespace Products.Services.Results;

public record UpdateProductResult(bool Success, ResultErrorReason ErrorReason, IDictionary<string, string> Errors)
{
    public static UpdateProductResult FromSuccess() =>
        new(true, ResultErrorReason.None, new Dictionary<string, string>(0));

    public static UpdateProductResult FromNotFoundError() =>
        new(false, ResultErrorReason.NotFound, new Dictionary<string, string>(0));

    public static UpdateProductResult FromValidationError(IDictionary<string, string> errors) => 
        new(false, ResultErrorReason.Validation, errors);
}
