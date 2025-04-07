import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface Movie {
  id: number;
  title: string;
  overview: string;
  posterPath: string;
  releaseDate: string;
  genres: string[];
  actors: string[];
  images: string[];
}

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private selectedMovieSubject = new BehaviorSubject<number | null>(null);
  selectedMovie$ = this.selectedMovieSubject.asObservable();
  private apiUrl = `${environment.apiUrl}/movie`;

  constructor(private http: HttpClient) { }

  getLatestMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.apiUrl}/latest`);
  }

  getTopRatedMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.apiUrl}/top-rated`);
  }

  searchMovies(searchType: string, keyword: string): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.apiUrl}/search`, { params: { searchType, keyword } });
  }

  getMovieDetails(movieId: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/details/${movieId}`);
  }

  setSelectedMovie(movieId: number): void {
    this.selectedMovieSubject.next(movieId);
  }
}
