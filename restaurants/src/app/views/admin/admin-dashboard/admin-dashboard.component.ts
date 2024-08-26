import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { UserModel } from 'src/app/models/user.model';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  public displayedColumnsOnUsers: string[] = ['name', 'action'];
  public users: UserModel[] = [];

  constructor(private usersService: UserService) {}

  ngOnInit() {
    this.loadUsers();
  }

  public deleteUser(id: string): void {
    this.usersService.delete(id).subscribe(() => {
      this.loadUsers();
    });
  }

  private loadUsers(): void {
    this.usersService.getAll().subscribe(r => {
      this.users = r;
    });
  }
}
