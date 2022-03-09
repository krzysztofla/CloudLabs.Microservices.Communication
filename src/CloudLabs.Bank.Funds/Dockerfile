FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CloudLabs.Bank.Funds/CloudLabs.Bank.Funds.csproj", "CloudLabs.Bank.Funds/"]
RUN dotnet restore "src/CloudLabs.Bank.Funds/CloudLabs.Bank.Funds.csproj"
COPY . .
WORKDIR "/src/CloudLabs.Bank.Funds"
RUN dotnet build "CloudLabs.Bank.Funds.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudLabs.Bank.Funds.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudLabs.Bank.Funds.dll"]
