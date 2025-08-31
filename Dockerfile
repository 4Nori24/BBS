FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app/BBSWebApp
COPY . ./
RUN dotnet publish BBSWebApp.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "BBSWebApp.dll"]