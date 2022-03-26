# Authenticating an ASP.NET Core 5 Web Application using IdentityServer 5

[![YouTube](https://img.youtube.com/vi/BlJd_LG_LC8/0.jpg)](https://www.youtube.com/watch?v=BlJd_LG_LC8)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:api,DNS:identityserver,DNS:webapplication"
    ```
    ```shell
    openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Authenticating an ASP.NET Core 5 Web Application using IdentityServer 5"
    ```
1. Import the self-signed certificate.
    ```shell
    certutil -f -user -importpfx Root localhost.pfx
    ```
1. Add the lines below to the hosts file.
    ```text
    127.0.0.1 api
    127.0.0.1 identityserver
    127.0.0.1 webapplication
    ```
1. Start the applications.
    ```shell
    docker compose up --build
    ```
1. Open the [API](https://webapplication:5005/api) page.
1. Enter the Username "test@example.com".
1. Enter the Password "Pass123$".
1. Click the Login button.
