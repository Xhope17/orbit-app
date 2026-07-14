import { ChangeDetectionStrategy, Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FollowService } from '../../../../../services/follow.service';
import { FollowInterface } from '../../../../../interfaces/follow.interface';
import { ProfileService } from '../../../../../services/profile.service';

@Component({
  selector: 'search-profile-component',
  imports: [RouterLink],
  templateUrl: './search-profile-component.html',
  styleUrl: './search-profile-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchProfileComponent {
  query = input.required<string>();

  private profileService = inject(ProfileService);
  private followService = inject(FollowService);

  public profiles = signal<FollowInterface[]>([]);
  public isLoading = signal<boolean>(true);

  //bloquear el botón de un usuario específico mientras carga su "Follow"
  public isToggling = signal<string[]>([]);

  constructor() {
    effect(
      () => {
        const currentQuery = this.query();
        if (currentQuery) {
          this.loadProfiles(currentQuery);
        }
      },
      { allowSignalWrites: true },
    );
  }

  loadProfiles(searchQuery: string): void {
    window.scrollTo({ top: 0, behavior: 'instant' });

    this.isLoading.set(true);

    this.profileService.searchProfiles(searchQuery).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.profiles.set(res.data.items);
        } else {
          this.profiles.set([]);
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error buscando personas', err);
        this.profiles.set([]);
        this.isLoading.set(false);
      },
    });
  }

  handleToggleFollow(profile: FollowInterface): void {
    // ignoramos clics adicionales
    if (this.isToggling().includes(profile.username)) return;

    this.isToggling.update((list) => [...list, profile.username]);

    if (profile.isFollowing) {
      // Dejar de seguir
      this.followService.unfollowUser(profile.username).subscribe({
        next: () => this.updateProfileState(profile.username, false),
        error: () => this.removeToggling(profile.username),
      });
    } else {
      // Seguir
      this.followService.followUser(profile.username).subscribe({
        next: () => this.updateProfileState(profile.username, true),
        error: () => this.removeToggling(profile.username),
      });
    }
  }

  // para la UI
  private updateProfileState(username: string, isFollowing: boolean) {
    this.profiles.update((current) =>
      current.map((p) => {
        if (p.username === username) {
          return { ...p, isFollowing };
        }
        return p;
      }),
    );
    this.removeToggling(username);
  }

  private removeToggling(username: string) {
    this.isToggling.update((list) => list.filter((u) => u !== username));
  }
}
