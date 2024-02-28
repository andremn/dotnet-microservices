namespace Products.Services.Results;

public record CreateProductResult(bool Success, int Id, ResultErrorReason ErrorReason, IDictionary<string, string> Errors)
{
    public static CreateProductResult FromSuccess(int id) =>
        new(true, id, ResultErrorReason.None, new Dictionary<string, string>(0));

    public static CreateProductResult FromError(
        IDictionary<string, string> errors,
        ResultErrorReason errorReason = ResultErrorReason.Validation) => new(false, 0, errorReason, errors);
}
