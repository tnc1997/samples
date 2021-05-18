# Adding an OAuth 2.0 Security Scheme to an ASP.NET Core API

[![YouTube](https://img.youtube.com/vi/eE_X8Y180zs/0.jpg)](https://www.youtube.com/watch?v=eE_X8Y180zs)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:api,DNS:identityserver"
    ```
    ```shell
    openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Adding an OAuth 2.0 Security Scheme to an ASP.NET Core API"
    ```
1. Import the self-signed certificate.
    ```shell
    certutil -f -user -importpfx Root localhost.pfx
    ```
1. Add the lines below to the hosts file.
    ```text
    127.0.0.1 api
    127.0.0.1 identityserver
    ```
1. Start the applications.
    ```shell
    docker compose up --build
    ```
1. Open the [Swagger UI](https://api:5003/swagger).
1. Click the Authorize button.
1. Enter the client_id "console".
1. Enter the client_secret "secret".
1. Select the "api" scope.
1. Click the Authorize button.
1. Click the Close button.
1. Send requests to the API.
