FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Kopieer de .csproj vanuit de submap naar de juiste structuur in de container
COPY ["Escapade.Booking/Escapade.Booking.csproj", "Escapade.Booking/"]

# 2. Voer restore uit op dat exacte pad
RUN dotnet restore "Escapade.Booking/Escapade.Booking.csproj"

# 3. Kopieer alle broncode
COPY . .

# 4. Navigeer naar de projectmap en bouw de Release
WORKDIR "/src/Escapade.Booking"
RUN dotnet publish "Escapade.Booking.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Escapade.Booking.dll"]