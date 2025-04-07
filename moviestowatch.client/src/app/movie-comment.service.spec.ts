import { TestBed } from '@angular/core/testing';

import { MovieCommentService } from './movie-comment.service';

describe('MovieCommentService', () => {
  let service: MovieCommentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MovieCommentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
