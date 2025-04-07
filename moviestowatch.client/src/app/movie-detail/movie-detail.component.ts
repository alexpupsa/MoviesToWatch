import { Component, OnInit } from '@angular/core';
import { Movie, MovieService } from '../movie.service';
import { environment } from '../../environments/environment';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-movie-detail',
  templateUrl: './movie-detail.component.html',
  styleUrls: ['./movie-detail.component.css']
})
export class MovieDetailComponent implements OnInit {
  movie: Movie | null = null;
  imageBasePath: string = '';
  isLoggedIn: boolean = false;

  constructor(private movieService: MovieService, private authService: AuthService) {
    this.imageBasePath = environment.tmdbApiImageBasePath;
  }

  ngOnInit(): void {
    this.movieService.selectedMovie$.subscribe(movieId => {
      if (movieId != null) {
        this.movieService.getMovieDetails(movieId).subscribe(movie => {
          this.movie = movie;
        });
      }
    });
    this.isLoggedIn = this.authService.isLoggedIn();
  }
}
