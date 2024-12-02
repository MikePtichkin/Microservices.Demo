FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Microservices.Demo.ReportService/Microservices.Demo.ReportService.csproj", "Microservices.Demo.ReportService/"]
COPY ["Microservices.Demo.ReportService.Infra/Microservices.Demo.ReportService.Infra.csproj", "Microservices.Demo.ReportService.Infra/"]
COPY ["Microservices.Demo.ReportService.Bll/Microservices.Demo.ReportService.Bll.csproj", "Microservices.Demo.ReportService.Bll/"]
COPY ["Microservices.Demo.ReportService.Domain/Microservices.Demo.ReportService.Domain.csproj", "Microservices.Demo.ReportService.Domain/"]

RUN dotnet restore "Microservices.Demo.ReportService/Microservices.Demo.ReportService.csproj"

COPY . .

WORKDIR "/src/Microservices.Demo.ReportService"

RUN dotnet build "Microservices.Demo.ReportService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.Demo.ReportService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Demo.ReportService.dll"]