FROM mcr.microsoft.com/dotnet/sdk:6.0 AS dotnet
WORKDIR /src
COPY "./IdentityServer.Bff/IdentityServer.Bff.csproj" "./IdentityServer.Bff/IdentityServer.Bff.csproj"
RUN dotnet restore "./IdentityServer.Bff/IdentityServer.Bff.csproj"
COPY "./IdentityServer.Bff" "./IdentityServer.Bff"
RUN dotnet publish "./IdentityServer.Bff/IdentityServer.Bff.csproj" -c Release -o /app/publish

FROM node:lts AS node
WORKDIR /src
COPY "./IdentityServer.Ui/package.json" "./IdentityServer.Ui/package.json"
COPY "./IdentityServer.Ui/package-lock.json" "./IdentityServer.Ui/package-lock.json"
RUN cd IdentityServer.Ui && npm install
COPY "./IdentityServer.Ui" "./IdentityServer.Ui"
RUN cd IdentityServer.Ui && npm run build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=dotnet "/app/publish" "."
COPY --from=node "/src/IdentityServer.Ui/build" "./wwwroot"
ENTRYPOINT ["dotnet", "IdentityServer.Bff.dll"]
