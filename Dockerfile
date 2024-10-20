# Usa la imagen oficial de .NET 8 para construir el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia el archivo .csproj y restaura las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia todo el contenido del proyecto y compila
COPY . ./
RUN dotnet publish -c Release -o out

# Usa una imagen más ligera de .NET 8 para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

# Exponer puertos (ajusta según sea necesario)
EXPOSE 80
EXPOSE 443
EXPOSE 5293

# Define el comando que se ejecutará cuando el contenedor inicie
ENTRYPOINT ["dotnet", "back-sv-users.dll"]
