# Usar la imagen de .NET 8 como base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5293

# Usar una imagen de .NET SDK 8.0 para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["back-sv-users.csproj", "./"]
RUN dotnet restore "./back-sv-users.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "back-sv-users.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "back-sv-users.csproj" -c Release -o /app/publish

# Configurar la aplicación para ejecutarse
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "back-sv-users.dll"]
