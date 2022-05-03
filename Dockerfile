FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY *.sln .
COPY API/*csproj ./API/
COPY Application/*csproj ./Application/
COPY Domain/*csproj ./Domain/
COPY Persistence/*csproj ./Persistence/
COPY Application.Tests/*csproj ./Application.Tests/

# Restore as distinct layers
RUN dotnet restore

COPY API/. ./API/
COPY Application/. ./Application/
COPY Domain/. ./Domain/
COPY Persistence/. ./Persistence/
COPY Application.Tests/. ./Application.Tests/

WORKDIR /app/API
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/API/out ./
ENTRYPOINT ["dotnet", "API.dll"]