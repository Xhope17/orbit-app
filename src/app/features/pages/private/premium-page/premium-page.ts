import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-premium-page',
  imports: [RouterLink],
  templateUrl: './premium-page.html',
  styleUrl: './premium-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PremiumPage {
  // Signal para manejar el estado del toggle (Anual vs Mensual)
  // True = Anual (con descuento), False = Mensual
  isAnnual = signal<boolean>(true);

  // Lista de beneficios para iterar en el HTML
  features = [
    { icon: 'fa-solid fa-certificate', text: 'Marca de verificación azul' },
    { icon: 'fa-solid fa-pen-to-square', text: 'Editar tus publicaciones' },
    { icon: 'fa-solid fa-text-height', text: 'Publicaciones más largas' },
    { icon: 'fa-solid fa-eye-slash', text: 'Mitad de anuncios en el feed' },
    { icon: 'fa-solid fa-arrow-up-right-dots', text: 'Mayor alcance y visibilidad' },
  ];
}
