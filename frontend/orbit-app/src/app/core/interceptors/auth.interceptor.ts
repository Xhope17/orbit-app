import {
  HttpErrorResponse,
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpEvent
} from '@angular/common/http';
import { inject } from '@angular/core';
import { throwError, BehaviorSubject, Observable } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { AuthService } from '../../shared/services/auth.service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  const tokenValido = token && !authService.isTokenExpired(token);
  if (tokenValido) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    });
  }

  return next(req).pipe(
    catchError((error: unknown) => {
      if (
        error instanceof HttpErrorResponse &&
        error.status === 401 &&
        !req.url.includes('/auth/login') &&
        !req.url.includes('/auth/refresh')
      ) {
        return handle401Error(req, next, authService);
      }

      return throwError(() => error);
    })
  );
};

function handle401Error(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService
): Observable<HttpEvent<unknown>> {

  if (!isRefreshing) {
    isRefreshing = true;
    refreshTokenSubject.next(null);

    const token = authService.getToken();
    const refreshToken = authService.getRefreshToken();

    if (token && refreshToken) {
      return authService.refreshToken(token, refreshToken).pipe(
        switchMap((res: any) => {
          isRefreshing = false;
          const newAccessToken = res.data?.accessToken;

          if (newAccessToken) {
            refreshTokenSubject.next(newAccessToken);
            return next(
              req.clone({
                setHeaders: { Authorization: `Bearer ${newAccessToken}` },
              })
            );
          }

          return throwError(() => new Error('No access token in response'));
        }),
        catchError((err) => {
          isRefreshing = false;
          authService.logout();
          return throwError(() => err);
        })
      );
    } else {
      isRefreshing = false;
      authService.logout();
      return throwError(() => new Error('Tokens missing'));
    }
  } else {
    return refreshTokenSubject.pipe(
      filter((token): token is string => token !== null),
      take(1),
      switchMap((jwt: string) => {
        return next(
          req.clone({
            setHeaders: { Authorization: `Bearer ${jwt}` },
          })
        );
      })
    );
  }
}
