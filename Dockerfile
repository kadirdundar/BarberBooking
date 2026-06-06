# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Proje dosyasını kopyala ve restore et
COPY ["BerberApp1/BerberApp1.csproj", "BerberApp1/"]
RUN dotnet restore "BerberApp1/BerberApp1.csproj"

# Tüm kodları kopyala
COPY . .
WORKDIR "/src/BerberApp1"

# Publish et
RUN dotnet publish "BerberApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Üretim ortamı için portu 80 yapıyoruz
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "BerberApp1.dll"]