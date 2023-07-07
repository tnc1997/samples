# Implementing Multitenancy in Entity Framework Core 6 using a Database Per-Tenant

[![YouTube](https://img.youtube.com/vi/t0pOYx9qU2U/0.jpg)](https://www.youtube.com/watch?v=t0pOYx9qU2U)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -nodes -newkey rsa:4096 -keyout example.local.key -out example.local.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:*.example.local"
    ```
    ```shell
    openssl pkcs12 -export -in example.local.crt -inkey example.local.key -out example.local.pfx -name "Implementing Multitenancy in Entity Framework Core 6 using a Database Per-Tenant"
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
