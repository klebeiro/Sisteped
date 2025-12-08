import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { ListClasses, Login, Register } from "@/pages/user";
import type { ReactElement } from "react";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Login />,
  },
  {
    path: "/register",
    element: <Register />,
  },
  {
    path: "/classes",
    element: <ListClasses />,
  }
]);

export function AppRouter(): ReactElement {
  return <RouterProvider router={router} />;
}
