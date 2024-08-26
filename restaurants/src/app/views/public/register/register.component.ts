import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserLoginModel } from 'src/app/models/user-login.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  public formGroup: FormGroup;
  public registrationFailed = false;
  public registrationSucceded = false;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.formGroup = new FormGroup({
      name: new FormControl('', [Validators.required]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(4)
      ])
    });
  }

  public register(): void {
    const registrationData: UserLoginModel = this.formGroup.getRawValue();
    this.userService.register(registrationData).subscribe(result => {
      this.registrationFailed = !result;
      this.registrationSucceded = result;
    });
  }
}
