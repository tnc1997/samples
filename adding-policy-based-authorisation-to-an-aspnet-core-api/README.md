# Adding Policy-Based Authorisation to an ASP.NET Core API

[![YouTube](https://img.youtube.com/vi/0gMNxDCw-Jw/0.jpg)](https://www.youtube.com/watch?v=0gMNxDCw-Jw)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:api,DNS:identityserver"
    ```
    ```shell
    openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Adding Policy-Based Authorisation to an ASP.NET Core API"
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
1. Start the services.
    ```shell
    docker compose up --build
    ```
