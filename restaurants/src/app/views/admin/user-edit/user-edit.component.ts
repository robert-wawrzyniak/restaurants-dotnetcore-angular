import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Urls } from 'src/app/models/urls';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss']
})
export class UserEditComponent implements OnInit {
  public formGroup: FormGroup;
  public id: string;

  constructor(
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');

    this.formGroup = new FormGroup({
      name: new FormControl('', [Validators.required]),
      isAdmin: new FormControl(''),
      isOwner: new FormControl('')
    });

    this.userService.getById(this.id).subscribe(u => {
      this.formGroup.setValue({
        name: u.name,
        isAdmin: u.isAdmin,
        isOwner: u.isOwner
      });
    });
  }

  public update(): void {
    this.userService
      .update(this.id, this.formGroup.getRawValue())
      .subscribe(() => {
        this.router.navigateByUrl(Urls.adminMain);
      });
  }
}
