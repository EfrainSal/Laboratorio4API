FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Laboratorio4.API/Laboratorio4.API.csproj Laboratorio4.API/
COPY domain/domain.csproj domain/
RUN dotnet restore Laboratorio4.API/Laboratorio4.API.csproj
COPY Laboratorio4.API/ Laboratorio4.API/
COPY domain/ domain/
RUN dotnet publish Laboratorio4.API/Laboratorio4.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish/ ./
ENTRYPOINT ["dotnet", "Laboratorio4.API.dll"]
