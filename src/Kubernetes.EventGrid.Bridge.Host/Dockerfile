FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Kubernetes.EventGrid.Bridge.Host/Kubernetes.EventGrid.Bridge.Host.csproj", "Kubernetes.EventGrid.Bridge.Host/"]
COPY ["Kubernetes.EventGrid.Core/Kubernetes.EventGrid.Core.csproj", "Kubernetes.EventGrid.Core/"]
RUN dotnet restore "Kubernetes.EventGrid.Bridge.Host/Kubernetes.EventGrid.Bridge.Host.csproj"
COPY . .
WORKDIR "/src/Kubernetes.EventGrid.Bridge.Host"
RUN dotnet build "Kubernetes.EventGrid.Bridge.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kubernetes.EventGrid.Bridge.Host.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/azure-functions/dotnet:4.0 AS runtime
WORKDIR /home/site/wwwroot
EXPOSE 80
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true