FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Microservices.Demo.DataGenerator/Microservices.Demo.DataGenerator.csproj", "Microservices.Demo.DataGenerator/"]
COPY ["Microservices.Demo.DataGenerator.Infra/Microservices.Demo.DataGenerator.Infra.csproj", "Microservices.Demo.DataGenerator.Infra/"]
COPY ["Microservices.Demo.DataGenerator.Bll/Microservices.Demo.DataGenerator.Bll.csproj", "Microservices.Demo.DataGenerator.Bll/"]
COPY ["Microservices.Demo.DataGenerator.Proto/Microservices.Demo.DataGenerator.Proto.csproj", "Microservices.Demo.DataGenerator.Proto/"]

RUN dotnet restore "Microservices.Demo.DataGenerator/Microservices.Demo.DataGenerator.csproj"

COPY . .
WORKDIR "/src/Microservices.Demo.DataGenerator"
RUN dotnet build "./Microservices.Demo.DataGenerator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Microservices.Demo.DataGenerator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Demo.DataGenerator.dll"]