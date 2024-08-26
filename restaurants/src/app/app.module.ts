import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './views/public/login/login.component';
import { RegisterComponent } from './views/public/register/register.component';
import { AuthorizationInterceptor } from './interceptors/authorization.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { UserDashboardComponent } from './views/user/user-dashboard/user-dashboard.component';
import { OwnerDashboardComponent } from './views/owner/owner-dashboard/owner-dashboard.component';
import { AdminDashboardComponent } from './views/admin/admin-dashboard/admin-dashboard.component';
import { RestaurantEditComponent } from './views/restaurant/restaurant-edit/restaurant-edit.component';
import { RestaurantDetailsComponent } from './views/restaurant/restaurant-details/restaurant-details.component';
import { ReviewEditComponent } from './views/reviews/review-edit/review-edit.component';
import { ReviewReplyComponent } from './views/reviews/review-reply/review-reply.component';
import { UserEditComponent } from './views/admin/user-edit/user-edit.component';

import { CookieService } from 'ngx-cookie-service';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    UserDashboardComponent,
    OwnerDashboardComponent,
    AdminDashboardComponent,
    RestaurantEditComponent,
    RestaurantDetailsComponent,
    ReviewEditComponent,
    ReviewReplyComponent,
    UserEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatCheckboxModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizationInterceptor,
      multi: true
    },
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
