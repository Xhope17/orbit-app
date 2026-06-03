import { ChangeDetectionStrategy, Component, input, output, signal } from '@angular/core';
import { ChatResponse } from '../../../../../interfaces/chat.interface';
import { DatePipe, UpperCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatAreaComponent } from '../chat-area-component/chat-area-component';

@Component({
  selector: 'chat-sidebar-component',
  imports: [DatePipe, UpperCasePipe, FormsModule],
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
