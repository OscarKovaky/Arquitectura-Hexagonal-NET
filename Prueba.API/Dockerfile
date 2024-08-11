FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Prueba.API/Prueba.API.csproj", "Prueba.API/"]
COPY ["Prueba.Application/Prueba.Application.csproj", "Prueba.Application/"]
COPY ["Prueba.Core/Prueba.Core.csproj", "Prueba.Core/"]
COPY ["Prueba.Infrastructure/Prueba.Infrastructure.csproj", "Prueba.Infrastructure/"]
RUN dotnet restore "Prueba.API/Prueba.API.csproj"
COPY . .
WORKDIR "/src/Prueba.API"
RUN dotnet build "Prueba.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Prueba.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prueba.API.dll"]
