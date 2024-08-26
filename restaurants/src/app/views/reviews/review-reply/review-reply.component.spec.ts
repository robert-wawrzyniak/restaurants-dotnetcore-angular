import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewReplyComponent } from './review-reply.component';

describe('ReviewReplyComponent', () => {
  let component: ReviewReplyComponent;
  let fixture: ComponentFixture<ReviewReplyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewReplyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewReplyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
