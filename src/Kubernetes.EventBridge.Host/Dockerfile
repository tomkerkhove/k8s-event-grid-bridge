FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.4 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2.204 AS build
WORKDIR /src
COPY ["Kubernetes.EventBridge.Host/Kubernetes.EventBridge.Host.csproj", "Kubernetes.EventBridge.Host/"]
RUN dotnet restore "Kubernetes.EventBridge.Host/Kubernetes.EventBridge.Host.csproj"
COPY . .
WORKDIR "/src/Kubernetes.EventBridge.Host"
RUN dotnet build "Kubernetes.EventBridge.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Kubernetes.EventBridge.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kubernetes.EventBridge.Host.dll"]