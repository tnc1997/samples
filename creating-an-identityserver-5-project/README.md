# Creating an IdentityServer 5 Project

[![YouTube](https://img.youtube.com/vi/4Odvc5mDyLg/0.jpg)](https://www.youtube.com/watch?v=4Odvc5mDyLg)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:identityserver"
    ```
    ```shell
    openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Creating an IdentityServer 5 Project"
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
1. Open the [Discovery Document](https://identityserver:5001/.well-known/openid-configuration).
