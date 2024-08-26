import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RestaurantService } from 'src/app/services/restaurant.service';
import { RestaurantModel } from 'src/app/models/restaurant.model';
import { Urls, urlWithParams } from 'src/app/models/urls';

@Component({
  selector: 'app-restaurant-edit',
  templateUrl: './restaurant-edit.component.html',
  styleUrls: ['./restaurant-edit.component.scss']
})
export class RestaurantEditComponent implements OnInit {
  public isExistingRestaurant: boolean;
  public formGroup: FormGroup;
  public id: string;

  constructor(
    private restaurantService: RestaurantService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.isExistingRestaurant = this.id !== null;

    this.formGroup = new FormGroup({
      name: new FormControl('', [Validators.required]),
      city: new FormControl('', [Validators.required]),
      address: new FormControl('', [Validators.required])
    });

    if (this.isExistingRestaurant) {
      this.restaurantService.getById(this.id).subscribe(r => {
        this.formGroup.setValue({
          name: r.name,
          city: r.city,
          address: r.address
        });
      });
    }
  }

  public save(): void {
    const restaurantData: RestaurantModel = this.formGroup.getRawValue();
    if (this.isExistingRestaurant) {
      this.restaurantService
        .updateRestaurant(this.id, restaurantData)
        .subscribe(() => {
          this.router.navigateByUrl(
            urlWithParams(Urls.restaurantDetails, { id: this.id })
          );
        });
    } else {
      this.restaurantService.createRestaurant(restaurantData).subscribe(id => {
        this.router.navigateByUrl(
          urlWithParams(Urls.restaurantDetails, { id })
        );
      });
    }
  }
}
