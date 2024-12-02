FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5125

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Microservices.Demo.CustomerService.csproj", "./"]
RUN dotnet restore "Microservices.Demo.CustomerService.csproj"
WORKDIR "/src"
COPY . .
RUN dotnet build "Microservices.Demo.CustomerService.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "Microservices.Demo.CustomerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Demo.CustomerService.dll"]