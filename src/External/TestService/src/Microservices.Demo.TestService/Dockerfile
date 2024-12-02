FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . ./

RUN dotnet restore "./src/Microservices.Demo.TestService/Microservices.Demo.TestService.csproj"

COPY . .

WORKDIR "/src/src/Microservices.Demo.TestService"
RUN dotnet build "./Microservices.Demo.TestService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Microservices.Demo.TestService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Demo.TestService.dll"]
