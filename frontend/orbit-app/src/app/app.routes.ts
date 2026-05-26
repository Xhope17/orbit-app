import { Routes } from '@angular/router';

export const routes: Routes = [
  // {
  //   path: 'login',
  //   loadComponent: () =>
  //     import('./features/pages/auth/login-page/login-page').then((m) => m.LoginPage),
  // },
  { path: 'auth', loadChildren: () => import('./features/pages/auth/auth.routes') },
];
