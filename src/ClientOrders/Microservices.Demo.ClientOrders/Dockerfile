FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Microservices.Demo.ClientOrders/Microservices.Demo.ClientOrders.csproj", "Microservices.Demo.ClientOrders/"]
COPY ["Microservices.Demo.ClientOrders.Infra/Microservices.Demo.ClientOrders.Infra.csproj", "Microservices.Demo.ClientOrders.Infra/"]
COPY ["Microservices.Demo.ClientOrders.Bll/Microservices.Demo.ClientOrders.Bll.csproj", "Microservices.Demo.ClientOrders.Bll/"]
COPY ["Microservices.Demo.ClientOrders.Domain/Microservices.Demo.ClientOrders.Domain.csproj", "Microservices.Demo.ClientOrders.Domain/"]
COPY ["Microservices.Demo.ClientOrders.Proto/Microservices.Demo.ClientOrders.Proto.csproj", "Microservices.Demo.ClientOrders.Proto/"]

RUN dotnet restore "Microservices.Demo.ClientOrders/Microservices.Demo.ClientOrders.csproj"

COPY . .

WORKDIR "/src/Microservices.Demo.ClientOrders"
RUN dotnet build "Microservices.Demo.ClientOrders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.Demo.ClientOrders.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Demo.ClientOrders.dll"]