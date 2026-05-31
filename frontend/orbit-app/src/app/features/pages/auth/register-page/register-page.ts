import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../shared/services/auth.service';
import { forbiddenUsernameValidator } from '../../../../shared/validators/username.validators';

@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './register-page.html',
  styleUrl: './register-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterPage {
  private _fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);

  public hasError = signal<boolean>(false);
  public errorMessage = signal<string>('');
  public isPosting = signal<boolean>(false);

  public registerForm = this._fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    username: ['', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(20),
      Validators.pattern(/^[a-z0-9._]+$/),
      forbiddenUsernameValidator(),
    ]],
    displayName: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [
      Validators.required,
      Validators.minLength(8),
      Validators.pattern(/(?=.*\d)/)
    ]]
  });

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.showError('Por favor, completa los campos correctamente.');
      return;
    }

    this.hasError.set(false);
    this.isPosting.set(true);

    const formValues = this.registerForm.getRawValue();

    const formData = new FormData();
    formData.append('email', formValues.email);
    formData.append('username', formValues.username);
    formData.append('displayName', formValues.displayName);
    formData.append('password', formValues.password);

    this.authService.register(formData).subscribe({
      next: (response) => {
        this.isPosting.set(false);
        if (response.isSuccess || response.message === 'User registered successfully') {
          this.router.navigateByUrl('/auth/login');
        } else {
          this.showError(response.message || 'Ocurrió un error en el registro.');
        }
      },
      error: (err) => {
        this.isPosting.set(false);

        // Atrapamos el mensaje principal del backend (Ideal para el 409 Conflict)
        let backendMsg = err.error?.message || 'Error de conexión con el servidor.';

        // Por si acaso se escapa una validación 400 y viene en el arreglo/diccionario
        if (err.error?.errors) {
          if (Array.isArray(err.error.errors)) {
            backendMsg = err.error.errors[0];
          } else {
            const firstKey = Object.keys(err.error.errors)[0];
            backendMsg = err.error.errors[firstKey][0];
          }
        }

        this.showError(backendMsg);
      }
    });
  }

  private showError(msg: string): void {
    this.errorMessage.set(msg);
    this.hasError.set(true);
    setTimeout(() => this.hasError.set(false), 4000);
  }
}
