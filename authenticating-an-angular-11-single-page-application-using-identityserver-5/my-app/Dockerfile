FROM node:12 as build
WORKDIR /src
COPY ["package.json", "package-lock.json", "./"]
RUN npm ci
COPY . .
RUN npm run build

FROM nginx:1 as final
COPY --from=build /src/dist/my-app /usr/share/nginx/html
EXPOSE 80
