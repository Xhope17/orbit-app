import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { UserProfile } from '../../../../../interfaces/user-profile.interface';

@Component({
  selector: 'quick-post-input-component',
  imports: [],
  templateUrl: './quick-post-input-component.html',
  styleUrl: './quick-post-input-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class QuickPostInputComponent {
  userProfile = input<UserProfile | null>(null);
  onOpenModal = output<void>();
}
