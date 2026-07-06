import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  effect,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { ChatService } from '../../../../services/chat.service';
import { SignalrService } from '../../../../../shared/services/signalr.service';
import { AuthService } from '../../../../../shared/services/auth.service';
import {
  ChatMessageBroadcast,
  ChatProfileInfo,
  ChatResponse,
  MessageResponse,
} from '../../../../interfaces/chat.interface';
import { DialogService } from '../../../../../shared/services/dialog.service';
import { ChatSidebarComponent } from '../components/chat-sidebar-component/chat-sidebar-component';
import { ChatAreaComponent } from '../components/chat-area-component/chat-area-component';

@Component({
  selector: 'app-chat-page',
  imports: [ChatSidebarComponent, ChatAreaComponent],
  templateUrl: './chat-page.html',
  styleUrl: './chat-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatPage implements OnInit {
  private readonly chatService = inject(ChatService);
  private readonly signalrService = inject(SignalrService);
  private readonly authService = inject(AuthService);
  private readonly dialogService = inject(DialogService);
  private readonly destroyRef = inject(DestroyRef);

  readonly conversations = signal<ChatResponse[]>([]);
  readonly messages = signal<MessageResponse[]>([]);
  readonly activeConversation = signal<ChatResponse | null>(null);
  readonly loadingConversations = signal(false);
  readonly loadingMessages = signal(false);
  readonly showMobileList = signal(true);
  readonly typingProfile = signal<ChatProfileInfo | null>(null);
  readonly currentProfileId = computed(() => this.authService.payload()?.profile_id ?? null);

  private typingTimeout: ReturnType<typeof setTimeout> | null = null;
  private joinedConversationId: string | null = null;

  constructor() {
    effect(() => {
      const msg = this.signalrService.onReceiveMessage();
      if (!msg) return;
      if (msg.conversationId === this.joinedConversationId) {
        this.messages.update((list) => [...list, this.toMessageResponse(msg)]);
        this.updateConversationLastMessage(msg);
        this.signalrService.markAsRead(msg.conversationId);
      } else {
        this.conversations.update((list) =>
          list.map((c) =>
            c.id === msg.conversationId
              ? {
                  ...c,
                  lastMessage: {
                    id: msg.id,
                    conversationId: msg.conversationId,
                    senderProfileId: msg.sender.profileId,
                    content: msg.content,
                    isSeen: msg.isSeen,
                    isEdited: msg.isEdited,
                    editedAt: msg.editedAt,
                    createdAt: msg.createdAt,
                    deletedAt: msg.deletedAt,
                    isFromCurrentUser: false,
                  },
                  unreadCount: c.unreadCount + 1,
                  isLastMessageFromCurrentUser: false,
                }
              : c,
          ),
        );
      }
    });

    effect(() => {
      const msg = this.signalrService.onReceiveOwnMessage();
      if (!msg) return;
      if (msg.conversationId === this.joinedConversationId) {
        this.messages.update((list) => [...list, this.toMessageResponse(msg)]);
        this.updateConversationLastMessage(msg);
      } else if (this.activeConversation()?.isPlaceholder) {
        const realId = msg.conversationId;
        this.joinedConversationId = realId;
        this.activeConversation.update((c) =>
          c ? { ...c, id: realId, isPlaceholder: false } : null,
        );
        this.messages.update((list) => [...list, this.toMessageResponse(msg)]);
        this.conversations.update((list) =>
          list.map((c) => (c.isPlaceholder ? { ...c, id: realId, isPlaceholder: false } : c)),
        );
        this.updateConversationLastMessage(msg);
        this.signalrService.joinConversation(realId);
      }
    });

    effect(() => {
      const conv = this.signalrService.onNewConversation();
      if (conv) {
        this.conversations.update((list) => {
          const exists = list.find((c) => c.id === conv.id);
          return exists ? list : [conv, ...list];
        });
      }
    });

    effect(() => {
      const event = this.signalrService.onMessageRead();
      if (event) {
        this.messages.update((list) =>
          list.map((m) =>
            m.senderProfileId !== this.currentProfileId() ? { ...m, isSeen: true } : m,
          ),
        );
        this.conversations.update((list) =>
          list.map((c) => (c.id === event.conversationId ? { ...c, unreadCount: 0 } : c)),
        );
      }
    });

    effect(() => {
      const event = this.signalrService.onUserTyping();
      if (event && event.conversationId === this.joinedConversationId) {
        const conv = this.conversations().find((c) => c.id === event.conversationId);
        if (conv && conv.otherParticipant.profileId === event.profileId) {
          this.typingProfile.set(conv.otherParticipant);
        }
        setTimeout(() => this.typingProfile.set(null), 3000);
      }
    });

    this.destroyRef.onDestroy(() => {
      if (this.joinedConversationId) {
        this.signalrService.leaveConversation(this.joinedConversationId);
      }
    });
  }

  ngOnInit(): void {
    this.loadConversations();
  }

  private updateConversationLastMessage(msg: ChatMessageBroadcast): void {
    this.conversations.update((list) =>
      list.map((c) =>
        c.id === msg.conversationId
          ? {
              ...c,
              lastMessage: {
                id: msg.id,
                conversationId: msg.conversationId,
                senderProfileId: msg.sender.profileId,
                content: msg.content,
                isSeen: msg.isSeen,
                isEdited: msg.isEdited,
                editedAt: msg.editedAt,
                createdAt: msg.createdAt,
                deletedAt: msg.deletedAt,
                isFromCurrentUser: msg.sender.profileId === this.currentProfileId(),
              },
              isLastMessageFromCurrentUser: msg.sender.profileId === this.currentProfileId(),
            }
          : c,
      ),
    );
  }

  private loadConversations(): void {
    this.loadingConversations.set(true);
    this.chatService.getConversations().subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.conversations.set(res.data);
        }
        this.loadingConversations.set(false);
      },
      error: () => this.loadingConversations.set(false),
    });
  }

  selectConversation(conversation: ChatResponse): void {
    if (this.joinedConversationId) {
      this.signalrService.leaveConversation(this.joinedConversationId);
    }
    this.activeConversation.set(conversation);
    this.showMobileList.set(false);
    this.joinedConversationId = conversation.id;
    if (conversation.isPlaceholder) return;
    this.signalrService.joinConversation(conversation.id);
    this.signalrService.markAsRead(conversation.id);
    this.loadMessages(conversation.id);
  }

  private loadMessages(conversationId: string): void {
    this.loadingMessages.set(true);
    this.chatService.getMessages(conversationId).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.messages.set(res.data.items);
        }
        this.loadingMessages.set(false);
      },
      error: () => this.loadingMessages.set(false),
    });
  }

  sendMessage(content: string): void {
    if (!content || !this.activeConversation()) return;
    const conv = this.activeConversation()!;
    const targetProfileId = conv.isPlaceholder ? conv.otherParticipant.profileId : undefined;
    this.signalrService.sendMessage(conv.id, content, targetProfileId);
  }

  onTyping(): void {
    if (!this.activeConversation()) return;
    if (this.typingTimeout) clearTimeout(this.typingTimeout);
    this.signalrService.typing(this.activeConversation()!.id);
    this.typingTimeout = setTimeout(() => {
      this.typingTimeout = null;
    }, 2000);
  }

  goBackToList(): void {
    if (this.joinedConversationId) {
      this.signalrService.leaveConversation(this.joinedConversationId);
      this.joinedConversationId = null;
    }
    this.activeConversation.set(null);
    this.messages.set([]);
    this.showMobileList.set(true);
  }

  createNewConversation(username: string): void {
    if (!username) return;
    this.chatService.createConversation({ username }).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.conversations.update((list) => {
            const exists = list.find((c) => c.id === res.data!.id);
            return exists ? list : [res.data!, ...list];
          });
          this.selectConversation(res.data);
        }
      },
      error: () => {
        this.dialogService.open({
          title: 'Error',
          message:
            'No se pudo crear la conversación. Verifica que el usuario exista y se sigan mutuamente.',
        });
      },
    });
  }

  private toMessageResponse(msg: ChatMessageBroadcast): MessageResponse {
    return {
      id: msg.id,
      conversationId: msg.conversationId,
      senderProfileId: msg.sender.profileId,
      content: msg.content,
      isSeen: msg.isSeen,
      isEdited: msg.isEdited,
      editedAt: msg.editedAt,
      createdAt: msg.createdAt,
      deletedAt: msg.deletedAt,
      isFromCurrentUser: msg.sender.profileId === this.currentProfileId(),
    };
  }
}
