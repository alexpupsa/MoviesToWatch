import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SearchComponent } from './search/search.component';
import { MovieDetailComponent } from './movie-detail/movie-detail.component';
import { MovieService } from './movie.service';
import { FormsModule } from '@angular/forms';
import { TopMoviesComponent } from './top-movies/top-movies.component';
import { LatestMoviesComponent } from './latest-movies/latest-movies.component';
import { MovieListComponent } from './movie-list/movie-list.component';
import { MovieCommentsComponent } from './movie-comments/movie-comments.component';
import { LoginComponent } from './login/login.component';
import { AuthInterceptor } from './auth-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    SearchComponent,
    MovieDetailComponent,
    TopMoviesComponent,
    LatestMoviesComponent,
    MovieListComponent,
    MovieCommentsComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [
    MovieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
