namespace Products.Services.Results;

public record CreateProductResult(bool Success, int Id, IDictionary<string, string> Errors)
{
    public static CreateProductResult FromSuccess(int id) =>
        new(true, id, new Dictionary<string, string>(0));

    public static CreateProductResult FromError(IDictionary<string, string> errors) =>
        new(false, 0, errors);
}
