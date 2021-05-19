FROM node:16 as build
WORKDIR /src
COPY "package.json" "package.json"
RUN npm install --legacy-peer-deps
COPY . .
RUN npm run build

FROM nginx:1 as final
COPY --from=build /src/dist/my-app /usr/share/nginx/html
EXPOSE 80
