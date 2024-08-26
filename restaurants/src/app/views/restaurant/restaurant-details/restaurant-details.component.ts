import { Component, OnInit } from '@angular/core';
import { RestaurantDetailsModel } from 'src/app/models/restaurant-details.model';
import { RestaurantService } from 'src/app/services/restaurant.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { Urls } from 'src/app/models/urls';

@Component({
  selector: 'app-restaurant-details',
  templateUrl: './restaurant-details.component.html',
  styleUrls: ['./restaurant-details.component.scss']
})
export class RestaurantDetailsComponent implements OnInit {
  public displayedColumnsOnReviews: string[] = [
    'user',
    'visitDate',
    'rate',
    'comment',
    'reply'
  ];
  public restaurant: RestaurantDetailsModel;

  constructor(
    private authorizationService: AuthorizationService,
    private restaurantService: RestaurantService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.restaurantService.getById(id).subscribe(r => {
      this.restaurant = r;
    });
  }

  public back(): void {
    this.authorizationService.getPermissions().subscribe(p => {
      if (p.isOwner) {
        this.router.navigateByUrl(Urls.ownerMain);
      } else {
        this.router.navigateByUrl(Urls.userMain);
      }
    });
  }
}
