# Creating an IdentityServer Project

## Getting Started

1. `openssl req -x509 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:identityserver"`
1. `openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Creating an IdentityServer Project"`
1. `certutil -f -user -importpfx Root localhost.pfx`
1. `docker compose up --build`
