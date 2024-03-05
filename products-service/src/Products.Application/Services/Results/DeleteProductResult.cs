namespace Products.Application.Services.Results;

public record DeleteProductResult(bool Success)
{
    public static DeleteProductResult FromSuccess() => new(true);

    public static DeleteProductResult FromNotFoundError() => new(false);
}
