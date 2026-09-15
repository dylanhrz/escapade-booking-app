FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopieer het .csproj bestand vanuit de submap
COPY ["Escapade.Booking/Escapade.Booking.csproj", "Escapade.Booking/"]
RUN dotnet restore "Escapade.Booking/Escapade.Booking.csproj"

# Kopieer de rest van de broncode
COPY . .
WORKDIR "/src/Escapade.Booking"
RUN dotnet publish "Escapade.Booking.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Escapade.Booking.dll"]