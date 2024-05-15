FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Alterar a variável de ambiente para a nova porta
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src .  

RUN dotnet restore "Golio.API/Golio.API.csproj"
WORKDIR "/src/Golio.API"
RUN dotnet build "Golio.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Golio.API.csproj" -c Release -o /app/publish

FROM base AS final
RUN addgroup --system appuser && adduser --system appuser --ingroup appuser
WORKDIR /app
COPY --from=publish /app/publish .
USER appuser
ENTRYPOINT ["dotnet", "Golio.API.dll"]
