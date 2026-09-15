FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopieer de .csproj bestanden direct vanuit de huidige werkmap
COPY ["Escapade.Booking.Core/Escapade.Booking.Core.csproj", "Escapade.Booking.Core/"]
COPY ["Escapade.Booking.Web/Escapade.Booking.Web.csproj", "Escapade.Booking.Web/"]

RUN dotnet restore "Escapade.Booking.Web/Escapade.Booking.Web.csproj"

COPY . .

WORKDIR "/src/Escapade.Booking.Web"
RUN dotnet publish "Escapade.Booking.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Escapade.Booking.Web.dll"]