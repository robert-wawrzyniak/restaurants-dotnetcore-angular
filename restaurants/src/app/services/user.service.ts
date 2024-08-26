import { Injectable } from '@angular/core';
import { UserLoginModel } from '../models/user-login.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { UserModel } from '../models/user.model';
import { ReviewModel } from '../models/review.model';
import { UserPermissionsModel } from '../models/user-permissions.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {}

  public getAll(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(`${environment.webApiUrl}/user`);
  }

  public getById(id: string): Observable<UserPermissionsModel> {
    return this.http.get<UserPermissionsModel>(
      `${environment.webApiUrl}/user/${id}`
    );
  }

  public getPendingReviewsForMe(): Observable<ReviewModel[]> {
    return this.http.get<ReviewModel[]>(
      `${environment.webApiUrl}/user/me/restaurants/reviews/pending`
    );
  }

  public register(registrationData: UserLoginModel): Observable<boolean> {
    return this.http
      .post(`${environment.webApiUrl}/user`, registrationData, {
        responseType: 'text'
      })
      .pipe(map(result => (result ? true : false)));
  }

  public update(
    id: string,
    userPermissions: UserPermissionsModel
  ): Observable<any> {
    return this.http.put(
      `${environment.webApiUrl}/user/${id}`,
      userPermissions
    );
  }

  public delete(id: string): Observable<any> {
    return this.http.delete(`${environment.webApiUrl}/user/${id}`);
  }
}
