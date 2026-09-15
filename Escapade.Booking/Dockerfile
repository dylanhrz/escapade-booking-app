FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Kopieer de .csproj bestanden van beide projecten
COPY ["Escapade.Booking.Core/Escapade.Booking.Core.csproj", "Escapade.Booking.Core/"]
COPY ["Escapade.Booking.Web/Escapade.Booking.Web.csproj", "Escapade.Booking.Web/"]

# 2. Herstel de afhankelijkheden via het hoofdproject
RUN dotnet restore "Escapade.Booking.Web/Escapade.Booking.Web.csproj"

# 3. Kopieer alle overige broncode
COPY . .

# 4. Bouw de Web applicatie
WORKDIR "/src/Escapade.Booking.Web"
RUN dotnet publish "Escapade.Booking.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Escapade.Booking.Web.dll"]