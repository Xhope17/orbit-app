import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';

@Component({
  selector: 'app-generic-dialog',
  imports: [],
  templateUrl: './generic-dialog.html',
  styleUrl: './generic-dialog.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GenericDialog {
  // Entradas para configurar el aspecto
  modalId = input.required<string>(); // Obligatorio para poder abrirlo
  title = input.required<string>();
  btnText = input<string>('Guardar');
  btnColor = input<string>('btn-primary'); // Clases de Daisy: btn-primary, btn-error, etc.

  // Salida cuando presionan el botón principal
  onSave = output<void>();

  // Método interno para cerrar el modal manualmente si es necesario
  closeModal() {
    const modal = document.getElementById(this.modalId()) as HTMLDialogElement;
    if (modal) modal.close();
  }
}
