FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["API_PUNTUALCHECK.csproj", "./"]
RUN dotnet restore "API_PUNTUALCHECK.csproj"
COPY . .
RUN dotnet build "API_PUNTUALCHECK.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API_PUNTUALCHECK.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "API_PUNTUALCHECK.dll"]