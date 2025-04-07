import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { AuthService } from './auth.service';

export interface MovieComment {
  id: number;
  movieId: number;
  comment: string;
  createdAt: string;
}

@Injectable({
  providedIn: 'root',
})
export class MovieCommentService {
  private apiUrl = `${environment.apiUrl}/comment`;

  constructor(private http: HttpClient, private authService: AuthService) { }

  getCommentsByMovieId(movieId: number): Observable<MovieComment[]> {
    return this.http.get<MovieComment[]>(`${this.apiUrl}/${movieId}`);
  }

  addComment(comment: MovieComment): Observable<MovieComment> {
    return this.http.post<MovieComment>(this.apiUrl, comment);
  }
}
