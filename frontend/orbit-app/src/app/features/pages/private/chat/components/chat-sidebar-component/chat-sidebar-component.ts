import { ChangeDetectionStrategy, Component, input, output, signal } from '@angular/core';
import { ChatResponse } from '../../../../../interfaces/chat.interface';
import { UpperCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LocalDatePipe } from '../../../../../../shared/pipes/local-date.pipe';
import { UserAvatarComponent } from '../../../../../../shared/components/user-avatar-component/user-avatar-component';

@Component({
  selector: 'chat-sidebar-component',
  imports: [UpperCasePipe, FormsModule, LocalDatePipe, UserAvatarComponent],
  templateUrl: './chat-sidebar-component.html',
  styleUrl: './chat-sidebar-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatSidebarComponent {
  conversations = input.required<ChatResponse[]>();
  loading = input<boolean>(false);

  onSelect = output<ChatResponse>();
  onCreateConv = output<string>();

  newUsername = signal('');

  handleCreate() {
    const val = this.newUsername().trim();
    if (val) {
      this.onCreateConv.emit(val);
      this.newUsername.set('');
    }
  }
}
