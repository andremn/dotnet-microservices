﻿namespace Products.Controllers.Products;

public record UpdateProductRequest(string Name, string Description, int Quantity, decimal Price);
