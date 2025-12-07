import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { Login, Register } from "@/pages/user";
import type { ReactElement } from "react";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Login />,
  },
  {
    path: "/register",
    element: <Register />,
  }
]);

export function AppRouter(): ReactElement {
  return <RouterProvider router={router} />;
}
