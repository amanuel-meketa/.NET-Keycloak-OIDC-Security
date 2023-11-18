#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DotNet-Keycloak.presentation/DotNet-Keycloak.presentation.csproj", "DotNet-Keycloak.presentation/"]
RUN dotnet restore "DotNet-Keycloak.presentation/DotNet-Keycloak.presentation.csproj"
COPY . .
WORKDIR "/src/DotNet-Keycloak.presentation"
RUN dotnet build "DotNet-Keycloak.presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNet-Keycloak.presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNet-Keycloak.presentation.dll"]