document.getElementById("loginForm").addEventListener("submit", async (event) => {
    event.preventDefault();
    
    const { userName, returnUrl } = Object.fromEntries(new FormData(event.target));
    
    let response = await fetch("/fido2/createassertionoptions", {
        body: JSON.stringify({ userName }),
        headers: { "Content-Type": "application/json" },
        method: "POST"
    });
    
    if (!response.ok) {
        throw new Error("Failed to create the assertion options.");
    }
    
    const publicKey = await response.json();
    
    publicKey.challenge = coerceToArrayBuffer(publicKey.challenge);
    
    for (const allowCredential of publicKey.allowCredentials) {
        allowCredential.id = coerceToArrayBuffer(allowCredential.id);
    }
    
    const credential = await navigator.credentials.get({ publicKey });
    
    response = await fetch("/fido2/createassertion", {
        body: JSON.stringify({
            id: credential.id,
            rawId: coerceToBase64Url(credential.rawId),
            type: credential.type,
            extensions: credential.getClientExtensionResults(),
            response: {
                authenticatorData: coerceToBase64Url(credential.response.authenticatorData),
                clientDataJSON: coerceToBase64Url(credential.response.clientDataJSON),
                signature: coerceToBase64Url(credential.response.signature)
            }
        }),
        headers: { "Content-Type": "application/json" },
        method: "POST"
    });
    
    if (!response.ok) {
        throw new Error("Failed to create the assertion.");
    }
    
    window.location.assign(returnUrl);
});
