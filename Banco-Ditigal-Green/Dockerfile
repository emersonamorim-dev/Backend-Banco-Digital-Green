#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Banco-Ditigal-Green/Banco-Ditigal-Green.csproj", "Banco-Ditigal-Green/"]
RUN dotnet restore "Banco-Ditigal-Green/Banco-Ditigal-Green.csproj"
COPY . .
WORKDIR "/src/Banco-Ditigal-Green"
RUN dotnet build "Banco-Ditigal-Green.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Banco-Ditigal-Green.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Banco-Ditigal-Green.dll"]