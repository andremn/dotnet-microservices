#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Products.Api/Products.Api.csproj", "Products.Api/"]
COPY ["src/Products.Application/Products.Application.csproj", "Products.Application/"]
COPY ["src/Products.Domain/Products.Domain.csproj", "Products.Domain/"]
COPY ["src/Products.Infrastructure/Products.Infrastructure.csproj", "Products.Infrastructure/"]
RUN dotnet restore "./Products.Api/Products.Api.csproj"
COPY /src .
WORKDIR "/src/Products.Api"
RUN dotnet build "./Products.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Products.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.Api.dll"]