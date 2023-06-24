# Adding Multitenancy to an ASP.NET Core 6 Application

[![YouTube](https://img.youtube.com/vi/VABZ7I3KRUM/0.jpg)](https://www.youtube.com/watch?v=VABZ7I3KRUM)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -nodes -newkey rsa:4096 -keyout example.local.key -out example.local.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:*.example.local"
    ```
    ```shell
    openssl pkcs12 -export -in example.local.crt -inkey example.local.key -out example.local.pfx -name "Adding Multitenancy to an ASP.NET Core 6 Application"
    ```
1. Import the self-signed certificate.
    ```shell
    certutil -f -user -importpfx Root example.local.pfx
    ```
1. Add the lines below to the hosts file.
    ```text
    127.0.0.1 one.example.local
    127.0.0.1 two.example.local
    ```
1. Start the services.
    ```shell
    docker compose up --build
    ```
