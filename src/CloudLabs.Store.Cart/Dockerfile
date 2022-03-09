FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CloudLabs.Store.Cart/CloudLabs.Store.Cart.csproj", "CloudLabs.Store.Cart/"]
RUN dotnet restore "src/CloudLabs.Store.Cart/CloudLabs.Store.Cart.csproj"
COPY . .
WORKDIR "/src/CloudLabs.Store.Cart"
RUN dotnet build "CloudLabs.Store.Cart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudLabs.Store.Cart.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudLabs.Store.Cart.dll"]
