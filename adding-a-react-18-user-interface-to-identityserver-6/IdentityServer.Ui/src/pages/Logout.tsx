import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";

export default function Logout() {
  const [searchParams] = useSearchParams();

  const [iFrameUrl, setIFrameUrl] = useState<string>();
  const [loading, setLoading] = useState(true);
  const [postLogoutRedirectUri, setPostLogoutRedirectUri] = useState<string>();

  useEffect(() => {
    fetch("/api/logout", {
      body: JSON.stringify({
        logoutId: searchParams.get("logoutId"),
      }),
      headers: {
        "Content-Type": "application/json",
      },
      method: "POST",
    })
      .then((response) => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error();
        }
      })
      .then(({ iFrameUrl, postLogoutRedirectUri } = {}) => {
        setIFrameUrl(iFrameUrl);
        setPostLogoutRedirectUri(postLogoutRedirectUri);
      })
      .finally(() => {
        setLoading(false);
      });
  }, [searchParams, setIFrameUrl, setLoading, setPostLogoutRedirectUri]);

  return loading ? (
    <p>Loading...</p>
  ) : (
    <>
      <h1>You are now logged out.</h1>
      {postLogoutRedirectUri && <p>Click <a href={postLogoutRedirectUri}>here</a> to return to the application.</p>}
      {iFrameUrl && <iframe height="0" src={iFrameUrl} width="0"></iframe>}
    </>
  );
}
