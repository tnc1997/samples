# Adding Passwordless Login to ASP.NET Core 8 Identity using FIDO2

[![YouTube](https://img.youtube.com/vi/3RZcHX1p7t8/0.jpg)](https://www.youtube.com/watch?v=3RZcHX1p7t8)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:webapplication"
    ```
    ```shell
    openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Adding Passwordless Login to ASP.NET Core 8 Identity using FIDO2"
    ```
1. Import the self-signed certificate.
    ```shell
    certutil -f -user -importpfx Root localhost.pfx
    ```
1. Add the line below to the hosts file.
    ```text
    127.0.0.1 webapplication
    ```
1. Start the services.
    ```shell
    docker compose up --build
    ```
