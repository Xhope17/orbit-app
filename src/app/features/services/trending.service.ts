import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { TrendingTopic } from '../interfaces/trending.interface';
import { ApiResponse } from '../../shared/interfaces/apiResponse.interface';

@Injectable({
  providedIn: 'root',
})
export class TrendingService {
  private http = inject(HttpClient);
  private readonly API = environment.apiUrl;

  getTrending(hours: number = 24) {
    let params = new HttpParams().set('hours', hours.toString());

    return this.http.get<ApiResponse<TrendingTopic[]>>(`${this.API}/trending`, { params });
  }
}
