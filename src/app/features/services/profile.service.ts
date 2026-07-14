import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/interfaces/apiResponse.interface';
import { FollowInterface } from '../interfaces/follow.interface';
import { PaginatedResponse } from '../../shared/interfaces/paginatedResult.interface';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  private http = inject(HttpClient);
  private readonly API = environment.apiUrl;

  searchProfiles(query: string, page: number = 1, pageSize: number = 20) {
    let params = new HttpParams()
      .set('q', query)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<ApiResponse<PaginatedResponse<FollowInterface>>>(
      `${this.API}/profiles/search`,
      { params },
    );
  }
}
