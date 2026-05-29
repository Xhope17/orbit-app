import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../shared/services/auth.service';
import { LoginRequest } from '../../../interfaces/login.interface';

@Component({
  selector: 'app-login-page',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginPage {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);

  public hasError = signal<boolean>(false);
  public errorMessage = signal<string>('');
  public isPosting = signal<boolean>(false);

  public loginForm = this.fb.nonNullable.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]],
  });

  onSubmit() {
    if (this.loginForm.invalid) {
      this.showError('Por favor, ingresa tu usuario y contraseña.');
      return;
    }

    this.isPosting.set(true);
    const { username, password } = this.loginForm.getRawValue();

    // Armamos el objeto respetando la interfaz
    const request: LoginRequest = {
      EmailOrUsername: username,
      password: password
    };

    this.authService.login(request).subscribe({
      next: () => {
        this.isPosting.set(false);
        this.router.navigateByUrl('/home');
      },
      error: (err) => {
        this.isPosting.set(false);
        // Extraemos el mensaje de error del backend si existe
        const msg = err.error?.message || 'Credenciales incorrectas.';
        this.showError(msg);
      },
    });
  }

  private showError(msg: string) {
    this.errorMessage.set(msg);
    this.hasError.set(true);
    setTimeout(() => {
      this.hasError.set(false);
    }, 3000);
  }
}
