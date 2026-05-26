import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './register-page.html',
  styleUrl: './register-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterPage {
  private _fb = inject(FormBuilder);
  public hasError = signal<boolean>(false);

  public registerForm = this._fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.minLength(3)]],
    username: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.hasError.set(true);

      setTimeout(() => this.hasError.set(false), 3000);
      return;
    }

    this.hasError.set(false);

    const userData = this.registerForm.getRawValue();

    console.log('✅ Formulario válido. Datos listos para la API de Orbit:', userData);

    // this._authService.register(userData).subscribe(...)
  }
}
