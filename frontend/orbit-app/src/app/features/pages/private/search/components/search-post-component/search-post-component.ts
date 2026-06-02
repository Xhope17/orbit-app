import { ChangeDetectionStrategy, Component, effect, inject, input, signal } from '@angular/core';
import { PostCardComponent } from '../../../../../../shared/components/post-card-component/post-card-component';
import { PostService } from '../../../../../services/post.service';
import { Subject, take } from 'rxjs';
import { AuthService } from '../../../../../../shared/services/auth.service';
import { DialogService } from '../../../../../../shared/services/dialog.service';
import { Post } from '../../../../../interfaces/post.interface';

@Component({
  selector: 'search-post-component',
  imports: [PostCardComponent],
  templateUrl: './search-post-component.html',
  styleUrl: './search-post-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchPostComponent {
  query = input.required<string>();

  private postService = inject(PostService);
  public authService = inject(AuthService);
  private dialogService = inject(DialogService);

  public posts = signal<Post[]>([]);
  public isLoading = signal<boolean>(true);
  public deletingPosts = signal<string[]>([]);

  constructor() {
    // El effect() se dispara cada vez que la señal query() cambia
    effect(
      () => {
        const currentQuery = this.query();
        if (currentQuery) {
          this.loadPosts(currentQuery);
        }
      },
      { allowSignalWrites: true },
    );
  }

  loadPosts(searchQuery: string): void {
    this.isLoading.set(true);
    this.postService.searchPosts(searchQuery).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.posts.set(res.data.items);
        } else {
          this.posts.set([]);
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error buscando posts', err);
        this.posts.set([]);
        this.isLoading.set(false);
      },
    });
  }


  handleDeletePost(postId: string): void {
    if (this.dialogService.data()) return;
    if (this.deletingPosts().includes(postId)) return;

    const confirmSubject = new Subject<void>();

    confirmSubject.pipe(take(1)).subscribe(() => {
      this.deletingPosts.update((ids) => [...ids, postId]);

      this.postService.deletePost(postId).subscribe({
        next: () => {
          this.posts.update((currentPosts) => currentPosts.filter((p) => p.id !== postId));
          this.deletingPosts.update((ids) => ids.filter((id) => id !== postId));
          this.dialogService.close();
        },
        error: (err) => {
          console.error('Error al eliminar', err);
          this.deletingPosts.update((ids) => ids.filter((id) => id !== postId));
          alert('Hubo un error al eliminar');
        },
      });
    });

    this.dialogService.open({
      title: '¿Eliminar publicación?',
      message:
        'Esta acción es permanente y no se puede deshacer. Se eliminará de tu perfil y de los resultados de búsqueda.',
      btnText: 'Eliminar',
      btnClass: 'btn-error text-white',
      onSave: confirmSubject,
    });
  }

  handleLikePost(postId: string): void {
    const post = this.posts().find((p) => p.id === postId);
    if (!post) return;

    if (post.isLiked) {
      this.postService.disLike(postId).subscribe({
        next: () => this.toggleLikeUI(postId),
      });
    } else {
      this.postService.toggleLike(postId).subscribe({
        next: () => this.toggleLikeUI(postId),
      });
    }
  }

  private toggleLikeUI(postId: string): void {
    this.posts.update((currentPosts) =>
      currentPosts.map((p) => {
        if (p.id === postId) {
          const increment = p.isLiked ? -1 : 1;
          return { ...p, likeCount: p.likeCount + increment, isLiked: !p.isLiked };
        }
        return p;
      }),
    );
  }
}
