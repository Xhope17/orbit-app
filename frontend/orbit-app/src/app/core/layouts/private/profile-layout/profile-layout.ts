import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLinkActive, RouterLink } from '@angular/router';
import { AuthService } from '../../../../shared/services/auth.service';
import { PostCardComponent } from '../../../../shared/components/post-card-component/post-card-component';
import { Post } from '../../../../features/interfaces/post.interface';

@Component({
  selector: 'app-profile-layout',
  imports: [RouterLink, RouterLinkActive, PostCardComponent],
  templateUrl: './profile-layout.html',
  styleUrl: './profile-layout.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileLayout {
  route = inject(ActivatedRoute);
  authService = inject(AuthService);

  // Lee el parámetro :username de la URL
  usernameUrl = signal<string>(this.route.snapshot.paramMap.get('username') || '');

  // Computed: Es true si el usuario logueado está viendo su propio perfil
  isMyProfile = computed(() => {
    const loggedUser = this.authService.username();
    return loggedUser === this.usernameUrl();
  });

  // Método para el Modal (Usa la API nativa de los dialogs de HTML5)
  openEditModal() {
    const modal = document.getElementById('edit_profile_modal') as HTMLDialogElement;
    if (modal) modal.showModal();
  }

  // Actualizado para cumplir estrictamente con la nueva interfaz Post de tu API
  userPosts = signal<Post[]>([
    {
      id: 'fc1ddb5c-a4d7-4772-8d68-4cadea7d3412', // GUID temporal
      author: {
        profileId: this.authService.payload()?.profile_id || 'mock-profile-id',
        username: this.usernameUrl(),
        displayName: 'Usuario Actual',
        avatarUrl: null
      },
      content: 'Probando mi nuevo perfil en Orbit. ¡Esto se ve genial!',
      media: [],
      likeCount: 10,
      commentCount: 2,
      isLiked: false,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    },
  ]);

  // Modificado para recibir string (GUID)
  handleDeletePost(postId: string) {
    this.userPosts.update((posts) => posts.filter((p) => p.id !== postId));
  }

  // Modificado para recibir string (GUID) y actualizar likeCount / isLiked
  handleLikePost(postId: string) {
    this.userPosts.update((posts) =>
      posts.map((p) => {
        if (p.id === postId) {
          // Lógica temporal para alternar el like (dar o quitar like)
          const increment = p.isLiked ? -1 : 1;
          return { ...p, likeCount: p.likeCount + increment, isLiked: !p.isLiked };
        }
        return p;
      }),
    );
  }
}
