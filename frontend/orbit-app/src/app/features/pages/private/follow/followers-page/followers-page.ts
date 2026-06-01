import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { EnContruccionComponent } from '../../../../../shared/components/en-contruccion-component/en-contruccion-component';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { FollowService } from '../../../../services/follow.service';
import { FollowInterface } from '../../../../interfaces/follow.interface';
import { Location, UpperCasePipe } from '@angular/common';
import { AuthService } from '../../../../../shared/services/auth.service';

@Component({
  selector: 'app-followers-page',
  imports: [RouterLink, UpperCasePipe],
  templateUrl: './followers-page.html',
  styleUrl: './followers-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FollowersPage implements OnInit {
  private route = inject(ActivatedRoute);
  private followService = inject(FollowService);
  private location = inject(Location);
  public authService = inject(AuthService);

  //id del usuario que se esta viendo en la url
  public loadingFollowIds = signal<Set<string>>(new Set());

  // para la UI
  public usernameUrl = signal<string>('');
  public followers = signal<FollowInterface[]>([]);
  public isLoading = signal<boolean>(true);

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      const username = params.get('username');
      if (username) {
        this.usernameUrl.set(username);
        this.loadFollowers(username);
      }
    });
  }

  loadFollowers(username: string) {
    this.isLoading.set(true);
    this.followService.getFollowers(username).subscribe({
      next: (resp) => {
        if (resp.isSuccess && resp.data) {
          this.followers.set(resp.data.items);
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error cargando seguidores', err);
        this.isLoading.set(false);
      },
    });
  }

  toggleFollow(event: Event, targetUser: FollowInterface) {
    event.stopPropagation();
    event.preventDefault();

    if (this.loadingFollowIds().has(targetUser.profileId)) return;

    this.setLoadingState(targetUser.profileId, true);

    if (targetUser.isFollowing) {
      this.followService.unfollowUser(targetUser.username).subscribe({
        next: () => this.updateUserFollowState(targetUser.profileId, false),
        error: () => this.setLoadingState(targetUser.profileId, false),
      });
    } else {
      this.followService.followUser(targetUser.username).subscribe({
        next: () => this.updateUserFollowState(targetUser.profileId, true),
        error: () => this.setLoadingState(targetUser.profileId, false),
      });
    }
  }

  private setLoadingState(profileId: string, isLoading: boolean) {
    this.loadingFollowIds.update((set) => {
      const newSet = new Set(set);
      isLoading ? newSet.add(profileId) : newSet.delete(profileId);
      return newSet;
    });
  }

  private updateUserFollowState(profileId: string, isFollowing: boolean) {
    this.followers.update((users) =>
      users.map((u) => (u.profileId === profileId ? { ...u, isFollowing } : u)),
    );
    this.setLoadingState(profileId, false);
  }

  goBack() {
    this.location.back();
  }
}
