import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { UserLoginModel } from 'src/app/models/user-login.model';
import { Router } from '@angular/router';
import { Urls } from 'src/app/models/urls';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public formGroup: FormGroup;
  public loginFailed = false;

  constructor(
    private authorizationService: AuthorizationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.formGroup = new FormGroup({
      name: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  public login(): void {
    if (this.formGroup.invalid) {
      return;
    }
    const loginData: UserLoginModel = this.formGroup.getRawValue();
    this.authorizationService.authenticate(loginData).subscribe(result => {
      this.loginFailed = !result;
      this.reditectUserToProperLandingPage();
    });
  }

  private reditectUserToProperLandingPage(): void {
    if (this.authorizationService.isAuthenticated) {
      this.authorizationService.getPermissions().subscribe(p => {
        if (p.isAdmin) {
          this.router.navigateByUrl(Urls.adminMain);
          return;
        }
        if (p.isOwner) {
          this.router.navigateByUrl(Urls.ownerMain);
          return;
        }
        this.router.navigateByUrl(Urls.userMain);
      });
    }
  }
}
