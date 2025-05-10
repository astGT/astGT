# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["AzureDevOpsMcp.csproj", "./"]
RUN dotnet restore

# Copy the rest of the files and build
COPY . .
RUN dotnet build "AzureDevOpsMcp.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "AzureDevOpsMcp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AzureDevOpsMcp.dll"]
