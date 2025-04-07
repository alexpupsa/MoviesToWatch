import { Component, Input } from '@angular/core';
import { Movie, MovieService } from '../movie.service';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.css']
})
export class MovieListComponent {
  @Input() movies: Movie[] = [];
  @Input() title: string = '';
  imageBasePath: string = '';

  constructor(private movieService: MovieService) {
    this.imageBasePath = environment.tmdbApiImageBasePath;
  }

  viewDetails(movieId: number): void {
    this.movieService.setSelectedMovie(movieId);
  }
}
