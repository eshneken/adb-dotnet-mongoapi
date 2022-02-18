FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY [".", "/src/"]


#WORKDIR /src/
RUN dotnet restore "adb-dotnet-mongoapi.csproj"
COPY . .
RUN dotnet build "adb-dotnet-mongoapi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "adb-dotnet-mongoapi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "adb-dotnet-mongoapi.dll"]
