import React, { FormEvent, useCallback } from "react";
import { useSearchParams } from "react-router-dom";

export default function Login() {
  const [searchParams] = useSearchParams();

  const handleSubmit = useCallback(
    async (event: FormEvent<HTMLFormElement>) => {
      event.preventDefault();

      const form = event.currentTarget;

      const formData = new FormData(form);

      const response = await fetch("/api/login", {
        body: JSON.stringify({
          userName: formData.get("userName"),
          password: formData.get("password"),
          returnUrl: searchParams.get("returnUrl"),
        }),
        headers: {
          "Content-Type": "application/json",
        },
        method: "POST",
      });

      if (response.ok) {
        const { returnUrl } = await response.json();

        window.location.href = returnUrl;
      } else {
        form.reset();
      }
    },
    [searchParams],
  );

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label htmlFor="userName">Username: </label>
        <input
          autoComplete="username"
          id="userName"
          name="userName"
          required
          type="text"
        />
      </div>

      <div>
        <label htmlFor="password">Password: </label>
        <input
          autoComplete="current-password"
          id="password"
          name="password"
          required
          type="password"
        />
      </div>

      <button type="submit">Log in</button>
    </form>
  );
}
