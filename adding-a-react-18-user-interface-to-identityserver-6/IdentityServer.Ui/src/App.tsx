import React from "react";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./App.css";
import Login from "./pages/Login";
import Logout from "./pages/Logout";

export default function App() {
  const router = createBrowserRouter([
    {
      path: "/account/login",
      element: <Login />,
    },
    {
      path: "/account/logout",
      element: <Logout />,
    },
  ]);

  return <RouterProvider router={router} />;
}
