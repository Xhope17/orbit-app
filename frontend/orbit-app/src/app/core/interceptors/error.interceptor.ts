import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      switch (error.status) {
        case 403:
          // No tiene permisos para esta acción específica
          router.navigate(['/home']);
          break;
        case 404:
          console.error('Recurso no encontrado:', req.url);
          break;
        case 500:
          console.error('Error interno del servidor en .NET');
          break;
      }
      return throwError(() => error);
    })
  );
};
