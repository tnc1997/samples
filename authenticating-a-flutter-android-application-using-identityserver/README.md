# Authenticating a Flutter Android Application using IdentityServer

[![YouTube](https://img.youtube.com/vi/Qai1AKptnZo/0.jpg)](https://www.youtube.com/watch?v=Qai1AKptnZo)

## Getting Started

1. Generate a self-signed certificate.
    ```shell
    openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:api,DNS:identityserver"
    ```
    ```shell
    openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Authenticating a Flutter Android Application using IdentityServer"
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
1. Update the `authorizationEndpoint` in the `main.dart` file.
    ```dart
    final authorizationEndpoint = Uri.parse(
      'https://<insert host here>/connect/authorize',
    );
    ```
1. Update the `tokenEndpoint` in the `main.dart` file.
    ```dart
    final tokenEndpoint = Uri.parse(
      'https://<insert host here>/connect/token',
    );
    ```
1. Run the Flutter application.
    ```shell
    flutter run
    ```
1. Wait for the application to start in the Android Emulator.
1. Enter the Username "test@example.com".
1. Enter the Password "Pass123$".
1. Click the Login button.
