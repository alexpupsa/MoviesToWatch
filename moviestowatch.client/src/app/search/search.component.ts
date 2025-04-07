import { Component, OnInit } from '@angular/core';
import { MovieService, Movie } from '../movie.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  title: string = '';
  genre: number = 0;
  searchResults: Movie[] = [];

  constructor(private movieService: MovieService) { }

  ngOnInit(): void { }

  search(searchType: string) {
    const keyword = searchType == 'title' ? this.title : this.genre.toString();
    this.movieService.searchMovies(searchType, keyword).subscribe(movies => {
      this.searchResults = movies;
    });
  }
}
