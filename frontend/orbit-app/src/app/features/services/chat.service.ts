import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/interfaces/apiResponse.interface';
import { PaginatedResponse } from '../../shared/interfaces/paginatedResult.interface';
import { ChatResponse, MessageResponse } from '../interfaces/chat.interface';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private readonly http = inject(HttpClient);
  private readonly API = environment.apiUrl;

  loadConversations(): Observable<ApiResponse<ChatResponse[]>> {
    return this.http.get<ApiResponse<ChatResponse[]>>(`${this.API}/chats`);
  }

  getMessages(
    conversationId: string,
  ): Observable<ApiResponse<PaginatedResponse<MessageResponse>>> {
    return this.http.get<ApiResponse<PaginatedResponse<MessageResponse>>>(
      `${this.API}/chats/${conversationId}/messages?page=1&pageSize=50`,
    );
  }

  createConversation(username: string): Observable<ApiResponse<ChatResponse>> {
    return this.http.post<ApiResponse<ChatResponse>>(`${this.API}/chats`, { username });
  }
}
