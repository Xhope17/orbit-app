import { ChangeDetectionStrategy, Component, inject, input, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UpperCasePipe } from '@angular/common';
import { DialogService } from '../../../../../../shared/services/dialog.service';
import { PostService } from '../../../../../services/post.service';
import { Post } from '../../../../../interfaces/post.interface';

@Component({
  selector: 'app-create-quote-modal',
  standalone: true,
  imports: [ReactiveFormsModule, UpperCasePipe],
  templateUrl: './create-quote-modal.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: 'flex-1 flex flex-col',
  },
})
export class CreateQuoteModal implements OnInit {
  private fb = inject(FormBuilder);
  private postService = inject(PostService);
  private dialogService = inject(DialogService);

  public originalPost = input.required<Post>();

  public isPosting = signal(false);
  public errorMessage = signal('');

  public quoteForm = this.fb.nonNullable.group({
    content: ['', [Validators.required, Validators.maxLength(1000)]],
  });

  ngOnInit() {
    const data = this.dialogService.data();
    if (data?.onSave) {
      data.onSave.subscribe(() => {
        this.submitQuote();
      });
    }
  }

  submitQuote() {
    if (this.isPosting()) return;

    const content = this.quoteForm.value.content?.trim();
    if (!content) {
      this.errorMessage.set('Agrega un comentario para citar.');
      return;
    }

    if (this.quoteForm.invalid) {
      this.errorMessage.set('El comentario excede el límite de caracteres.');
      return;
    }

    this.isPosting.set(true);
    this.errorMessage.set('');

    this.postService.threadPost(this.originalPost().id, content).subscribe({
      next: (res) => {
        this.isPosting.set(false);
        if (res.isSuccess && res.data) {
          const data = this.dialogService.data();
          if (data?.onSuccess) {
            data.onSuccess.next(res.data);
          }
          this.dialogService.close();
        }
      },
      error: (err) => {
        this.isPosting.set(false);
        this.errorMessage.set(err.error?.message || 'Error al citar.');
      },
    });
  }
}
