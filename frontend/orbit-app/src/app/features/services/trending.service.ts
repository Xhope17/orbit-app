import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { TrendingTopic } from '../interfaces/trending.interface';

@Injectable({
  providedIn: 'root',
})
export class TrendingService {
  private http = inject(HttpClient);
  private readonly API = environment.apiUrl;

  getTrending(hours: number = 24) {
    let params = new HttpParams().set('hours', hours.toString());

    // 2. ADIÓS AL ANY. Le decimos que el backend devolverá un arreglo de TrendingTopic
    return this.http.get<TrendingTopic[]>(`${this.API}/trending`, { params });
  }
}
