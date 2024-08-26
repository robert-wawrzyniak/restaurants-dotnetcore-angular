import { Injectable } from '@angular/core';
import { CanActivate, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizationService } from '../services/authorization.service';
import { map } from 'rxjs/operators';
import { Urls } from '../models/urls';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(
    private router: Router,
    private authorizationService: AuthorizationService
  ) {}

  canActivate(): Observable<boolean | UrlTree> {
    return this.authorizationService.getPermissions().pipe(
      map(permissions => {
        const isUserAllowed = permissions.isAdmin;
        if (!isUserAllowed) {
          this.router.navigateByUrl('/' + Urls.userMain);
        }
        return isUserAllowed;
      })
    );
  }
}
