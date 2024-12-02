FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Microservices.Demo.OrdersFacade/Microservices.Demo.OrdersFacade.csproj", "Microservices.Demo.OrdersFacade/"]
COPY ["Microservices.Demo.OrdersFacade.Infra/Microservices.Demo.OrdersFacade.Infra.csproj", "Microservices.Demo.OrdersFacade.Infra/"]
COPY ["Microservices.Demo.OrdersFacade.Application/Microservices.Demo.OrdersFacade.Application.csproj", "Microservices.Demo.OrdersFacade.Application/"]
COPY ["Microservices.Demo.OrdersFacade.Domain/Microservices.Demo.OrdersFacade.Domain.csproj", "Microservices.Demo.OrdersFacade.Domain/"]

RUN dotnet restore "Microservices.Demo.OrdersFacade/Microservices.Demo.OrdersFacade.csproj"

COPY . .

WORKDIR "/src/Microservices.Demo.OrdersFacade"
RUN dotnet build "Microservices.Demo.OrdersFacade.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.Demo.OrdersFacade.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Demo.OrdersFacade.dll"]