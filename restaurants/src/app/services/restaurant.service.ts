import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RestaurantModel } from '../models/restaurant.model';
import { ReviewModel } from '../models/review.model';
import { RestaurantDetailsModel } from '../models/restaurant-details.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  constructor(private http: HttpClient) {}

  public getAll(): Observable<RestaurantModel[]> {
    return this.http.get<RestaurantModel[]>(
      `${environment.webApiUrl}/restaurant`
    );
  }

  public getYours(): Observable<RestaurantModel[]> {
    return this.http.get<RestaurantModel[]>(
      `${environment.webApiUrl}/restaurant/forcurrentuser`
    );
  }

  public getById(id: string): Observable<RestaurantDetailsModel> {
    return this.http.get<RestaurantDetailsModel>(
      `${environment.webApiUrl}/restaurant/${id}`
    );
  }

  public createRestaurant(restaurant: RestaurantModel): Observable<string> {
    return this.http
      .post(`${environment.webApiUrl}/restaurant`, restaurant, {
        responseType: 'text'
      })
      .pipe(map(s => s.substr(1, s.length - 2)));
  }

  public updateRestaurant(
    id: string,
    restaurant: RestaurantModel
  ): Observable<any> {
    return this.http.put(
      `${environment.webApiUrl}/restaurant/${id}`,
      restaurant,
      {
        responseType: 'text'
      }
    );
  }

  public deleteRestaurant(id: string): Observable<any> {
    return this.http.delete(`${environment.webApiUrl}/restaurant/${id}`);
  }

  public getReview(
    restaurantId: string,
    userId: string
  ): Observable<ReviewModel> {
    return this.http.get<ReviewModel>(
      `${environment.webApiUrl}/restaurant/${restaurantId}/reviews/${userId}`
    );
  }

  public reviewRestaurant(
    restaurantId: string,
    review: ReviewModel
  ): Observable<any> {
    return this.http.post<string>(
      `${environment.webApiUrl}/restaurant/${restaurantId}/reviews`,
      review
    );
  }

  public replyToReview(
    restaurantId: string,
    userId: string,
    reply: string
  ): Observable<any> {
    return this.http.post(
      `${environment.webApiUrl}/restaurant/${restaurantId}/reviews/${userId}/reply`,
      {
        reply
      }
    );
  }

  public deleteReview(restaurantId: string, userId: string): Observable<any> {
    return this.http.delete(
      `${environment.webApiUrl}/restaurant/${restaurantId}/reviews/${userId}`
    );
  }
}
