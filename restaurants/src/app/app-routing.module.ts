import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './views/public/login/login.component';
import { RegisterComponent } from './views/public/register/register.component';
import { Urls } from './models/urls';
import { AuthGuard } from './guards/auth.guard';
import { UserDashboardComponent } from './views/user/user-dashboard/user-dashboard.component';
import { OwnerGuard } from './guards/owner.guard';
import { OwnerDashboardComponent } from './views/owner/owner-dashboard/owner-dashboard.component';
import { AdminGuard } from './guards/admin.guard';
import { AdminDashboardComponent } from './views/admin/admin-dashboard/admin-dashboard.component';
import { UserEditComponent } from './views/admin/user-edit/user-edit.component';
import { RestaurantDetailsComponent } from './views/restaurant/restaurant-details/restaurant-details.component';
import { RestaurantEditComponent } from './views/restaurant/restaurant-edit/restaurant-edit.component';
import { ReviewEditComponent } from './views/reviews/review-edit/review-edit.component';
import { ReviewReplyComponent } from './views/reviews/review-reply/review-reply.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', component: LoginComponent },
      { path: Urls.register, component: RegisterComponent }
    ]
  },
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      { path: Urls.userMain, component: UserDashboardComponent },
      { path: Urls.restaurantDetails, component: RestaurantDetailsComponent },
      { path: Urls.review, component: ReviewEditComponent }
    ]
  },
  {
    path: '',
    canActivate: [OwnerGuard],
    children: [
      { path: Urls.ownerMain, component: OwnerDashboardComponent },
      { path: Urls.restaurantAdd, component: RestaurantEditComponent },
      { path: Urls.restaurantEdit, component: RestaurantEditComponent },
      { path: Urls.reviewReply, component: ReviewReplyComponent }
    ]
  },
  {
    path: '',
    canActivate: [AdminGuard],
    children: [
      { path: Urls.adminMain, component: AdminDashboardComponent },
      { path: Urls.adminEditUser, component: UserEditComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
