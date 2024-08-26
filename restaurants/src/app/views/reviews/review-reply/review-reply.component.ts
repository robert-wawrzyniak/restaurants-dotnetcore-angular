import { Component, OnInit } from '@angular/core';
import { RestaurantService } from 'src/app/services/restaurant.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ReviewModel } from 'src/app/models/review.model';
import { Urls } from 'src/app/models/urls';

@Component({
  selector: 'app-review-reply',
  templateUrl: './review-reply.component.html',
  styleUrls: ['./review-reply.component.scss']
})
export class ReviewReplyComponent implements OnInit {
  public review: ReviewModel;
  public reply: string;
  public restaurantId: string;
  public userId: string;

  constructor(
    private restaurantService: RestaurantService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.restaurantId = this.route.snapshot.paramMap.get('restaurantId');
    this.userId = this.route.snapshot.paramMap.get('userId');
  }

  ngOnInit() {
    this.restaurantService
      .getReview(this.restaurantId, this.userId)
      .subscribe(r => {
        this.review = r;
      });
  }

  public save(): void {
    this.restaurantService
      .replyToReview(this.restaurantId, this.userId, this.reply)
      .subscribe(() => {
        this.router.navigateByUrl(Urls.ownerMain);
      });
  }
}
