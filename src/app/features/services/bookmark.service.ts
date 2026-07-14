import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/interfaces/apiResponse.interface';
import { PaginatedResponse } from '../../shared/interfaces/paginatedResult.interface';
import { Post, BookmarkResponse } from '../interfaces/post.interface';

@Injectable({
  providedIn: 'root',
})
export class BookmarkService {
  private http = inject(HttpClient);
  private readonly API = environment.apiUrl;


  getSavedPosts(page: number = 1, pageSize: number = 20) {
    let params = new HttpParams().set('page', page.toString()).set('pageSize', pageSize.toString());

    return this.http.get<ApiResponse<PaginatedResponse<Post>>>(`${this.API}/saved-posts`, {
      params,
    });
  }

  // Guardar un post
  savePost(postId: string) {
    return this.http.post<ApiResponse<BookmarkResponse>>(`${this.API}/saved-posts/${postId}`, {});
  }

  // eliminar de guardados
  unSavePost(postId: string) {
    return this.http.delete<ApiResponse<BookmarkResponse>>(`${this.API}/saved-posts/${postId}`);
  }
}
