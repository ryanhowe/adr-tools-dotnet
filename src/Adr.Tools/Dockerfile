FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Adr.Tools/Adr.Tools.csproj", "src/Adr.Tools/"]
RUN dotnet restore "src/Adr.Tools/Adr.Tools.csproj"
COPY . .
WORKDIR "/src/src/Adr.Tools"
RUN dotnet build "Adr.Tools.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adr.Tools.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adr.Tools.dll"]
