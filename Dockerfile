# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy solution and projects
COPY *.sln ./
COPY ComplyTest.API/*.csproj ./ComplyTest.API/
COPY ComplyTest.Application/*.csproj ./ComplyTest.Application/
COPY ComplyTest.Domain/*.csproj ./ComplyTest.Domain/
COPY ComplyTest.Infrastructure/*.csproj ./ComplyTest.Infrastructure/
COPY ComplyTest.Test/*.csproj ./ComplyTest.Test/

# Restore
RUN dotnet restore

# Copy everything
COPY . ./

# Publish
RUN dotnet publish ComplyTest.API/ComplyTest.API.csproj -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published app
COPY --from=build-env /app/out ./

# Create Images directory
RUN mkdir -p /app/Images

# Optional: curl for healthchecks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

EXPOSE 8080
ENTRYPOINT ["dotnet", "ComplyTest.API.dll"]