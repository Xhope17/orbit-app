import { ChangeDetectionStrategy, Component, inject, input, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { DialogService } from '../../../../../../shared/services/dialog.service';
import {
  Community,
  CreateCommunityPayload,
  UpdateCommunityPayload,
} from '../../../../../interfaces/community-interface';
import { CommunityService } from '../../../../../services/community.service';

@Component({
  selector: 'create-community-modal-component',
  imports: [ReactiveFormsModule],
  templateUrl: './create-community-modal-component.html',
  styleUrl: './create-community-modal-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateCommunityModalComponent implements OnInit {
  private fb = inject(FormBuilder);
  private communityService = inject(CommunityService);
  private dialogService = inject(DialogService);

  // NUEVO: Input opcional para saber si estamos editando
  public communityToEdit = input<Community>();

  public isSubmitting = signal(false);
  public errorMessage = signal('');

  public communityForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    description: ['', [Validators.maxLength(500)]],
    isPrivate: [false],
  });

  ngOnInit() {
    // llenamos los campos si estamos en modo edición
    const editData = this.communityToEdit();
    if (editData) {
      this.communityForm.patchValue({
        name: editData.name,
        description: editData.description || '',
        isPrivate: editData.isPrivate,
      });

      // se bloquea el campo nombre
      this.communityForm.controls.name.disable();
    }

    const data = this.dialogService.data();
    if (data?.onSave) {
      data.onSave.subscribe(() => {
        this.submitForm();
      });
    }
  }

  submitForm() {
    if (this.communityForm.invalid || this.isSubmitting()) return;

    this.isSubmitting.set(true);
    this.errorMessage.set('');

    const editData = this.communityToEdit();

    // editar
    if (editData) {
      const updatePayload: UpdateCommunityPayload = {
        name: this.communityForm.getRawValue().name.trim(),
        description: this.communityForm.value.description?.trim() || null,
        isPrivate: this.communityForm.value.isPrivate,
      };

      this.communityService.updateCommunity(editData.slug, updatePayload).subscribe({
        next: (res) => this.handleSuccess(res),
        error: (err) => this.handleError(err, 'Error al actualizar la comunidad.'),
      });
    }
    // crear nueva comunidad
    else {
      const createPayload: CreateCommunityPayload = {
        name: this.communityForm.value.name!.trim(),
        description: this.communityForm.value.description?.trim() || null,
        isPrivate: this.communityForm.value.isPrivate!,
      };

      this.communityService.createCommunity(createPayload).subscribe({
        next: (res) => this.handleSuccess(res),
        error: (err) => this.handleError(err, 'Error al crear la comunidad.'),
      });
    }
  }

  private handleSuccess(res: any) {
    this.isSubmitting.set(false);
    if (res.isSuccess && res.data) {
      const dialogData = this.dialogService.data();
      if (dialogData?.onSuccess) {
        dialogData.onSuccess.next(res.data);
      }
      this.dialogService.close();
    }
  }

  private handleError(err: any, defaultMsg: string) {
    this.isSubmitting.set(false);
    this.errorMessage.set(err.error?.message || defaultMsg);
  }
}
