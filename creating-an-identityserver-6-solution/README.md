# Creating an IdentityServer 6 Solution

[![YouTube](https://img.youtube.com/vi/jKMK4LgeSSc/0.jpg)](https://www.youtube.com/watch?v=jKMK4LgeSSc)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:api,DNS:identityserver,DNS:singlepageapplication,DNS:webapplication"
    ```
    ```shell
    openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Creating an IdentityServer 6 Solution"
    ```
1. Import the self-signed certificate.
    ```shell
    certutil -f -user -importpfx Root localhost.pfx
    ```
1. Add the line below to the hosts file.
    ```text
    127.0.0.1 api
    127.0.0.1 identityserver
    127.0.0.1 singlepageapplication
    127.0.0.1 webapplication
    ```
1. Start the services.
    ```shell
    docker compose up --build
    ```
