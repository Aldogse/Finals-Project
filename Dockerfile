# Use an official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project files to the container
COPY ["Property and Supply Management/Property and Supply Management.csproj", "Property and Supply Management/"]

# Restore dependencies from the specified NuGet source
RUN dotnet restore "Property and Supply Management/Property and Supply Management.csproj" --source https://api.nuget.org/v3/index.json

# Copy the rest of the source code
COPY . .

# Build the app
RUN dotnet build "Property and Supply Management/Property and Supply Management.csproj" -c Release --no-restore

# Publish the app
RUN dotnet publish "Property and Supply Management/Property and Supply Management.csproj" -c Release -o /app/publish --no-build

# Use the base image to copy the build artifacts into the container
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Property and Supply Management.dll"]
