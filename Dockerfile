# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["BerberApp1/BerberApp1.csproj", "BerberApp1/"]
RUN dotnet restore "BerberApp1/BerberApp1.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/BerberApp1"

# Build and publish the application
RUN dotnet publish "BerberApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variables for production
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BerberApp1.dll"]
