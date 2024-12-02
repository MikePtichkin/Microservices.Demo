FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Microservices.Demo.ViewOrder/Microservices.Demo.ViewOrder.csproj", "Microservices.Demo.ViewOrder/"]

COPY ["Microservices.Demo.ViewOrder.Infra/Microservices.Demo.ViewOrder.Infra.csproj", "Microservices.Demo.ViewOrder.Infra/"]
COPY ["Microservices.Demo.ViewOrder.Bll/Microservices.Demo.ViewOrder.Bll.csproj", "Microservices.Demo.ViewOrder.Bll/"]
COPY ["Microservices.Demo.ViewOrder.Domain/Microservices.Demo.ViewOrder.Domain.csproj", "Microservices.Demo.ViewOrder.Domain/"]
COPY ["Microservices.Demo.ViewOrder.Proto/Microservices.Demo.ViewOrder.Proto.csproj", "Microservices.Demo.ViewOrder.Proto/"]
COPY ["Microservices.Demo.ViewOrder.ShardConfiguration/Microservices.Demo.ViewOrder.ShardConfiguration.csproj", "Microservices.Demo.ViewOrder.ShardConfiguration/"]
COPY ["Microservices.Demo.ViewOrder.ShardedMigrator/Microservices.Demo.ViewOrder.ShardedMigrator.csproj", "Microservices.Demo.ViewOrder.ShardedMigrator/"]

RUN dotnet restore "Microservices.Demo.ViewOrder/Microservices.Demo.ViewOrder.csproj"

COPY . .

WORKDIR "/src/Microservices.Demo.ViewOrder"
RUN dotnet build "Microservices.Demo.ViewOrder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.Demo.ViewOrder.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Demo.ViewOrder.dll"]