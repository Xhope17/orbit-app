import { DatePipe, UpperCasePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PostComment } from '../../../features/interfaces/post.interface';

@Component({
  selector: 'comment-item-component',
  imports: [RouterLink, DatePipe, UpperCasePipe],
  templateUrl: './comment-item-component.html',
  styleUrl: './comment-item-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommentItemComponent {
  // Recibimos el comentario y el ID del usuario actual (para saber si mostrar el basurero)
  comment = input.required<PostComment>();
  currentUserId = input<string | null>(null);

  // Emitimos el ID cuando se quiera eliminar
  onDelete = output<string>();

  handleDelete() {
    // Aquí también podrías llamar a tu GenericDialog si quieres confirmación para borrar comentarios
    this.onDelete.emit(this.comment().id);
  }
}
