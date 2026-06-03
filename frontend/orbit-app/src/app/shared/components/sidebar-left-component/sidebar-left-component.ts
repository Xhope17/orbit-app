import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Subject } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../../features/services/user.service';
import { SignalrService } from '../../services/signalr.service';
import { NotificationService } from '../../../features/services/notification.service';
import { UserProfile } from '../../../features/interfaces/user-profile.interface';
import { Post } from '../../../features/interfaces/post.interface';
import { DialogService } from '../../services/dialog.service';
import { CreatePostModal } from '../create-post-modal/create-post-modal';

interface MenuItem {
  icon: string;
  label: string;
  route: string;
  badge?: number | null;
}

@Component({
  selector: 'app-sidebar-left',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar-left-component.html',
  styleUrl: './sidebar-left-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarLeftComponent implements OnInit {
  public authService = inject(AuthService);
  public userService = inject(UserService);
  private readonly signalrService = inject(SignalrService);
  private readonly notificationService = inject(NotificationService);

  private readonly dialogService = inject(DialogService);

  public userProfile = signal<UserProfile | null>(null);
  public isAuthenticated = this.authService.isAuthenticated;
  public username = this.authService.username;
  public notificationCount = signal(0);

  public publicMenu: MenuItem[] = [
    { icon: 'fa-regular fa-house', label: 'Inicio', route: '/home' },
    { icon: 'fa-solid fa-magnifying-glass', label: 'Explorar', route: '/search' },
  ];

  public privateMenu = computed<MenuItem[]>(() => [
    { icon: 'fa-regular fa-bookmark', label: 'Guardados', route: '/bookmarks' },
    {
      icon: 'fa-regular fa-bell',
      label: 'Notificaciones',
      route: '/i/notifications',
      badge: this.notificationCount(),
    },
    { icon: 'fa-regular fa-comment', label: 'Chat', route: '/i/chat' },
    { icon: 'fa-solid fa-users', label: 'Comunidades', route: '/community' },
    {
      icon: 'fa-regular fa-address-book',
      label: 'Seguidores',
      route: `/${this.username()}/followers`,
    },
    { icon: 'fa-regular fa-gem', label: 'Premium', route: '/i/premium' },
    { icon: 'fa-regular fa-user', label: 'Perfil', route: `/${this.username()}` },
  ]);

  constructor() {
    effect(() => {
      const notif = this.signalrService.onNotification();
      if (notif) {
        this.notificationCount.update((c) => c + 1);
      }
    });
  }

  ngOnInit(): void {
    this.loadNotificationCount();
    if (this.isAuthenticated()) {
      this.authService.getCurrentUser().subscribe({
        next: (res) => {
          if (res.isSuccess && res.data) {
            this.userProfile.set(res.data);
          }
        },
        error: (err) => console.error('Error al cargar perfil', err),
      });
    }
  }

  private loadNotificationCount(): void {
    this.notificationService.getUnreadCount().subscribe({
      next: (res) => {
        if (res.isSuccess && res.data !== undefined) {
          this.notificationCount.set(res.data);
        }
      },
    });
  }

  abrirModalPost() {
    const saveSubject = new Subject<void>();
    const successSubject = new Subject<Post>();

    this.dialogService.open({
      title: 'Nuevo Post',
      component: CreatePostModal,
      btnText: 'Postear',
      onSave: saveSubject,
      onSuccess: successSubject,
    });
  }

  handleLogout(): void {
    this.signalrService.stopConnections();
    this.authService.logout();
  }
}
