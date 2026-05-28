import { Routes } from "@angular/router";
import { AuthLayout } from "../../../core/layouts/auth/auth-layout/auth-layout";
import { LoginPage } from "./login-page/login-page";
import { RegisterPage } from "./register-page/register-page";

export const authRoutes: Routes = [
  {
    path: '',
    component: AuthLayout,
    children: [
      {
        path: 'login',
        component: LoginPage,
      },
      {
        path: 'register',
        component: RegisterPage,
      },
      {
        path: '**',
        redirectTo: 'login',
      },
    ],
  },
];

export default authRoutes
