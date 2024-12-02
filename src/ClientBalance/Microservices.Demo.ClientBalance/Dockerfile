FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Microservices.Demo.ClientBalance/Microservices.Demo.ClientBalance.csproj", "Microservices.Demo.ClientBalance/"]
COPY ["Microservices.Demo.ClientBalance.Infra/Microservices.Demo.ClientBalance.Infra.csproj", "Microservices.Demo.ClientBalance.Infra/"]
COPY ["Microservices.Demo.ClientBalance.Bll/Microservices.Demo.ClientBalance.Bll.csproj", "Microservices.Demo.ClientBalance.Bll/"]
COPY ["Microservices.Demo.ClientBalance.Domain/Microservices.Demo.ClientBalance.Domain.csproj", "Microservices.Demo.ClientBalance.Domain/"]

RUN dotnet restore "Microservices.Demo.ClientBalance/Microservices.Demo.ClientBalance.csproj"

COPY . .

WORKDIR "/src/Microservices.Demo.ClientBalance"
RUN dotnet build "Microservices.Demo.ClientBalance.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.Demo.ClientBalance.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Demo.ClientBalance.dll"]