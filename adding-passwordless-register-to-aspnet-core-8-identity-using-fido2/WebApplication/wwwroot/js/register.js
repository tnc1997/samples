document.getElementById("registerForm").addEventListener("submit", async (event) => {
    event.preventDefault();
    
    const { userName, returnUrl } = Object.fromEntries(new FormData(event.target));
    
    let response = await fetch("/fido2/createattestationoptions", {
        body: JSON.stringify({ userName }),
        headers: { "Content-Type": "application/json" },
        method: "POST"
    });
    
    if (!response.ok) {
        throw new Error("Failed to create the attestation options.");
    }
    
    const publicKey = await response.json();
    
    publicKey.challenge = coerceToArrayBuffer(publicKey.challenge);
    publicKey.user.id = coerceToArrayBuffer(publicKey.user.id);
    
    for (const excludeCredential of publicKey.excludeCredentials) {
        excludeCredential.id = coerceToArrayBuffer(excludeCredential.id);
    }
    
    if (publicKey.authenticatorSelection.authenticatorAttachment === null) {
        publicKey.authenticatorSelection.authenticatorAttachment = undefined;
    }
    
    const credential = await navigator.credentials.create({ publicKey });
    
    response = await fetch("/fido2/createattestation", {
        body: JSON.stringify({
            id: credential.id,
            rawId: coerceToBase64Url(credential.rawId),
            type: credential.type,
            extensions: credential.getClientExtensionResults(),
            response: {
                attestationObject: coerceToBase64Url(credential.response.attestationObject),
                clientDataJSON: coerceToBase64Url(credential.response.clientDataJSON),
                transports: credential.response.getTransports()
            }
        }),
        headers: { "Content-Type": "application/json" },
        method: "POST"
    });
    
    if (!response.ok) {
        throw new Error("Failed to create the attestation.");
    }
    
    window.location.assign(returnUrl);
});
