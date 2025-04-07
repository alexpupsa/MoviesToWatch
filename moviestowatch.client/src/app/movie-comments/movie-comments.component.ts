import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { MovieCommentService, MovieComment } from '../movie-comment.service';

@Component({
  selector: 'app-movie-comments',
  templateUrl: './movie-comments.component.html',
  styleUrls: ['./movie-comments.component.css'],
})
export class MovieCommentsComponent implements OnInit, OnChanges {
  @Input() movieId: number | null = null;
  comments: MovieComment[] = [];
  newComment: string = '';

  constructor(private movieCommentService: MovieCommentService) { }

  ngOnInit(): void {
    this.loadComments();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.comments = [];
    if (changes['movieId']) {
      this.loadComments();
    }
  }

  loadComments(): void {
    if (this.movieId) {
      this.movieCommentService.getCommentsByMovieId(this.movieId).subscribe(comments => {
        this.comments = comments
      });
    }
  }

  addComment(): void {
    if (this.movieId) {
      const comment: MovieComment = {
        id: 0,
        movieId: this.movieId,
        comment: this.newComment,
        createdAt: new Date().toISOString(),
      };

      this.movieCommentService.addComment(comment).subscribe(newComment => {
        this.comments.push(newComment);
        this.newComment = '';
      });
    }
  }
}
