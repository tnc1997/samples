# Seeding an ASP.NET Core 5 Identity Database with Users

[![YouTube](https://img.youtube.com/vi/Vo4hk-cfJh0/0.jpg)](https://www.youtube.com/watch?v=Vo4hk-cfJh0)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:identityserver"
    ```
    ```shell
    openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Seeding an ASP.NET Core 5 Identity Database with Users"
    ```
1. Import the self-signed certificate.
    ```shell
    certutil -f -user -importpfx Root localhost.pfx
    ```
1. Add the line below to the hosts file.
    ```text
    127.0.0.1 identityserver
    ```
1. Start the services.
    ```shell
    docker compose up --build
    ```
1. Open the [diagnostics](https://identityserver:5001/diagnostics) page.
1. Enter the Username "test@example.com".
1. Enter the Password "Pass123$".
1. Click the Login button.
