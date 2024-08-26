import { Component } from '@angular/core';
import { AuthorizationService } from './services/authorization.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(
    private authorizationService: AuthorizationService,
    private router: Router
  ) {}

  title = 'restaurants';

  public get isAuthenticated(): boolean {
    return this.authorizationService.isAuthenticated;
  }

  public logout(): void {
    this.authorizationService.logout();
    this.router.navigateByUrl('/');
  }
}
