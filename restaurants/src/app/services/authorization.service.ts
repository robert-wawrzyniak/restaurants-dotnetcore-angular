import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { UserPermissionsModel } from '../models/user-permissions.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserLoginModel } from '../models/user-login.model';
import { catchError } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  private readonly tokenCookieName = 'restaurants-jwt-token';
  private readonly token$: BehaviorSubject<string> = new BehaviorSubject<
    string
  >(null);
  private readonly userPermissions$: BehaviorSubject<
    UserPermissionsModel
  > = new BehaviorSubject<UserPermissionsModel>(null);

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  public get isAuthenticated(): boolean {
    return this.token$.value || this.loadTokenFromCookie() ? true : false;
  }

  public getToken(): Observable<string> {
    if (this.token$.value || this.loadTokenFromCookie()) {
      return of(this.token$.value);
    }

    return new Observable<string>(s => {
      const subscription = this.token$.subscribe(t => {
        if (t) {
          s.next(t);
          s.complete();
          subscription.unsubscribe();
        }
      });
    });
  }

  public getPermissions(): Observable<UserPermissionsModel> {
    if (this.userPermissions$.value) {
      return of(this.userPermissions$.value);
    }

    return new Observable<UserPermissionsModel>(s => {
      const subscription = this.userPermissions$.subscribe(p => {
        if (p) {
          s.next(p);
          s.complete();
          subscription.unsubscribe();
        }
      });
    });
  }

  public authenticate(loginData: UserLoginModel): Observable<boolean> {
    return new Observable<boolean>(s => {
      this.http
        .post(`${environment.webApiUrl}/user/token`, loginData, {
          responseType: 'text'
        })
        .pipe(
          catchError((err, caught) => {
            s.next(false);
            s.complete();

            return caught;
          })
        )
        .subscribe((token: string) => {
          this.token$.next(token);
          this.loadUserPermissions();

          s.next(true);
          s.complete();

          this.cookieService.set(
            this.tokenCookieName,
            token,
            new Date(Date.now() + 4 * 3600 * 1000)
          );
        });
    });
  }

  public logout(): void {
    this.token$.next(null);
    this.userPermissions$.next(null);
    this.cookieService.delete(this.tokenCookieName);
  }

  private loadTokenFromCookie(): boolean {
    const token = this.cookieService.get(this.tokenCookieName);
    if (token) {
      this.token$.next(token);
      this.loadUserPermissions();
      return true;
    }
    return false;
  }

  private loadUserPermissions(): void {
    this.http
      .get<UserPermissionsModel>(`${environment.webApiUrl}/user/me`)
      .subscribe(result => {
        this.userPermissions$.next(result);
      });
  }
}
