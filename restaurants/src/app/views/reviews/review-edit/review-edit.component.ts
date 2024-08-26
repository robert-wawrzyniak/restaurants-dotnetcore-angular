import { Component, OnInit } from '@angular/core';
import { ReviewModel } from 'src/app/models/review.model';
import { RestaurantService } from 'src/app/services/restaurant.service';
import { Router, ActivatedRoute } from '@angular/router';
import { urlWithParams, Urls } from 'src/app/models/urls';

@Component({
  selector: 'app-review-edit',
  templateUrl: './review-edit.component.html',
  styleUrls: ['./review-edit.component.scss']
})
export class ReviewEditComponent implements OnInit {
  public review: ReviewModel = { rate: 0 } as ReviewModel;

  constructor(
    private restaurantService: RestaurantService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  public icon(i: number): string {
    return i > this.review.rate ? 'star_border' : 'star';
  }

  public isStarSelected(i: number): boolean {
    return i <= this.review.rate;
  }

  public clickedStar(i: number): void {
    this.review.rate = i;
  }

  public save(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.restaurantService.reviewRestaurant(id, this.review).subscribe(() => {
      this.router.navigateByUrl(urlWithParams(Urls.restaurantDetails, { id }));
    });
  }

  ngOnInit() {}
}
