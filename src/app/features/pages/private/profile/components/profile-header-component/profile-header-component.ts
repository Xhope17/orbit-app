import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { UserProfile } from '../../../../../interfaces/user-profile.interface';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-profile-header',
  imports: [RouterLink],
  templateUrl: './profile-header-component.html',
  styleUrl: './profile-header-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileHeader {
  currentProfile = input<UserProfile | null>(null);
  usernameUrl = input<string>('');
  isMyProfile = input<boolean>(false);
  editProfile = output<void>();

  //spinner de carga en botón
  isTogglingFollow = input<boolean>(false);
  //seguir y dejar de seguir
  toggleFollow = output<void>();
}
