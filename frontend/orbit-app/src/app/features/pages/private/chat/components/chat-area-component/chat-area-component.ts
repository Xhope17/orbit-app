import { UpperCasePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  effect,
  ElementRef,
  input,
  output,
  signal,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LocalDatePipe } from '../../../../../../shared/pipes/local-date.pipe';
import {
  ChatResponse,
  MessageResponse,
  ChatProfileInfo,
} from '../../../../../interfaces/chat.interface';

@Component({
  selector: 'chat-area-component',
  imports: [UpperCasePipe, FormsModule, LocalDatePipe],
  templateUrl: './chat-area-component.html',
  styleUrl: './chat-area-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatAreaComponent {
  conversation = input.required<ChatResponse>();
  messages = input.required<MessageResponse[]>();
  currentProfileId = input.required<string | null>();
  loading = input<boolean>(false);
  typingProfile = input<ChatProfileInfo | null>(null);

  onSendMessage = output<string>();
  onTyping = output<void>();
  onGoBack = output<void>();

  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;
  messageInput = signal('');

  constructor() {
    effect(() => {
      const msgs = this.messages();
      setTimeout(() => this.scrollToBottom(), 50);
    });
  }

  isOwn(msg: MessageResponse): boolean {
    return msg.senderProfileId === this.currentProfileId();
  }

  handleSend() {
    const val = this.messageInput().trim();
    if (val) {
      this.onSendMessage.emit(val);
      this.messageInput.set('');
    }
  }

  private scrollToBottom() {
    if (this.scrollContainer) {
      const el = this.scrollContainer.nativeElement;
      el.scrollTop = el.scrollHeight;
    }
  }
}
