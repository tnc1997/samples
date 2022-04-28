FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApplication/ConsoleApplication.csproj", "ConsoleApplication/"]
RUN dotnet restore "ConsoleApplication/ConsoleApplication.csproj"
COPY . .
WORKDIR "/src/ConsoleApplication"
RUN dotnet build "ConsoleApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApplication.dll"]
