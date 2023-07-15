FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Zeepkist.Voximity.Proxy/Zeepkist.Voximity.Proxy.csproj", "Zeepkist.Voximity.Proxy/"]
RUN dotnet restore "Zeepkist.Voximity.Proxy/Zeepkist.Voximity.Proxy.csproj"
COPY . .
WORKDIR "/src/Zeepkist.Voximity.Proxy"
RUN dotnet build "Zeepkist.Voximity.Proxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zeepkist.Voximity.Proxy.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zeepkist.Voximity.Proxy.dll"]
