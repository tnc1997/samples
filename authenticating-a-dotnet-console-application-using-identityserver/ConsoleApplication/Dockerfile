FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY "ConsoleApplication.csproj" "ConsoleApplication.csproj"
RUN dotnet restore "ConsoleApplication.csproj"
COPY . .
RUN dotnet build "ConsoleApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApplication.dll"]
