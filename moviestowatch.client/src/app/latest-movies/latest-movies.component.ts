import { Component, OnInit } from '@angular/core';
import { MovieService, Movie } from '../movie.service';

@Component({
  selector: 'app-latest-movies',
  templateUrl: './latest-movies.component.html',
  styleUrls: ['./latest-movies.component.css']
})
export class LatestMoviesComponent implements OnInit {
  movies: Movie[] = [];

  constructor(private movieService: MovieService) { }

  ngOnInit(): void {
    this.movieService.getLatestMovies().subscribe(movies => {
      this.movies = movies;
    });
  }
}
