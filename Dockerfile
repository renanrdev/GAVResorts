# Use a imagem oficial da Microsoft para ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use a imagem SDK para compilar o código
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GAVResorts.csproj", "./"]
RUN dotnet restore "./GAVResorts.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "GAVResorts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GAVResorts.csproj" -c Release -o /app/publish

# Copie os arquivos publicados para a imagem base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GAVResorts.dll"]
