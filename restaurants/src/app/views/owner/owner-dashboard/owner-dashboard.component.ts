import { Component, OnInit } from '@angular/core';
import { RestaurantModel } from 'src/app/models/restaurant.model';
import { RestaurantService } from 'src/app/services/restaurant.service';
import { ReviewModel } from 'src/app/models/review.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-owner-dashboard',
  templateUrl: './owner-dashboard.component.html',
  styleUrls: ['./owner-dashboard.component.scss']
})
export class OwnerDashboardComponent implements OnInit {
  public displayedColumnsOnRestaurants: string[] = [
    'name',
    'city',
    'address',
    'action'
  ];
  public displayedColumnsOnReviews: string[] = [
    'restaurant',
    'user',
    'visitDate',
    'rate',
    'comment',
    'action'
  ];
  public restaurants: RestaurantModel[] = [];
  public pendingReviews: ReviewModel[] = [];

  constructor(
    private restaurantService: RestaurantService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.restaurantService.getYours().subscribe(r => {
      this.restaurants = r;
    });
    this.userService.getPendingReviewsForMe().subscribe(r => {
      this.pendingReviews = r;
    });
  }
}
