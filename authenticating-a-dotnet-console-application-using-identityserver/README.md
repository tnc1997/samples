# Authenticating a .NET Console Application using IdentityServer

## Getting Started

1. `openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:api,DNS:identityserver"`
1. `openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Authenticating a .NET Console Application using IdentityServer"`
1. `certutil -f -user -importpfx Root localhost.pfx`
1. `docker compose up --build`
