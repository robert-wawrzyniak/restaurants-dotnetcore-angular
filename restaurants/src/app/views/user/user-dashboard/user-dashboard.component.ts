import { Component, OnInit } from '@angular/core';
import { RestaurantModel } from 'src/app/models/restaurant.model';
import { RestaurantService } from 'src/app/services/restaurant.service';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss']
})
export class UserDashboardComponent implements OnInit {
  public displayedColumns: string[] = ['name', 'city', 'address', 'action'];
  public restaurants: RestaurantModel[] = [];

  constructor(private restaurantService: RestaurantService) {}

  ngOnInit() {
    this.restaurantService.getAll().subscribe(r => {
      this.restaurants = r;
    });
  }
}
