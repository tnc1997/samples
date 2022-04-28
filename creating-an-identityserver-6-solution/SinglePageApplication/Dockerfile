FROM nginx:1 AS base
WORKDIR /usr/share/nginx/html
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SinglePageApplication/SinglePageApplication.csproj", "SinglePageApplication/"]
RUN dotnet restore "SinglePageApplication/SinglePageApplication.csproj"
COPY . .
WORKDIR "/src/SinglePageApplication"
RUN dotnet build "SinglePageApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SinglePageApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
