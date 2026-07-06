import { UpperCasePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
  selector: 'user-avatar-component',
  imports: [UpperCasePipe],
  templateUrl: './user-avatar-component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserAvatarComponent {
  avatarUrl = input<string | null>(null);
  displayName = input.required<string>();
  size = input<number>(10);
}
