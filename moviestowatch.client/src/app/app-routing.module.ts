import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from './search/search.component';
import { MovieDetailComponent } from './movie-detail/movie-detail.component';
import { TopMoviesComponent } from './top-movies/top-movies.component';
import { LatestMoviesComponent } from './latest-movies/latest-movies.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
  { path: 'top-movies', component: TopMoviesComponent },
  { path: 'latest-movies', component: LatestMoviesComponent },
  { path: 'search', component: SearchComponent },
  { path: 'movie/:id', component: MovieDetailComponent },
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/top-movies', pathMatch: 'full' }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
