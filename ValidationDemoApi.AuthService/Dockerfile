FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ValidationDemoApi.AuthService/ValidationDemoApi.AuthService.csproj", "ValidationDemoApi.AuthService/"]
RUN dotnet restore "ValidationDemoApi.AuthService/ValidationDemoApi.AuthService.csproj"
COPY . .
WORKDIR "/src/ValidationDemoApi.AuthService"
RUN dotnet build "ValidationDemoApi.AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ValidationDemoApi.AuthService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ValidationDemoApi.AuthService.dll"]
