import { Injectable } from '@angular/core';
import { CanActivate, UrlTree, Router } from '@angular/router';
import { AuthorizationService } from '../services/authorization.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authorizationService: AuthorizationService
  ) {}

  public canActivate(): boolean | UrlTree {
    return this.authorizationService.isAuthenticated
      ? true
      : this.router.parseUrl('');
  }
}
