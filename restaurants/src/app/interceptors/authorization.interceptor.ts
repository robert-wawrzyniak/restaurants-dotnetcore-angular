import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';
import { AuthorizationService } from '../services/authorization.service';
import { Observable, of } from 'rxjs';
import { mergeMap } from 'rxjs/operators';

@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor {
  constructor(public authorizationService: AuthorizationService) {}

  public intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (!this.authorizationService.isAuthenticated) {
      return next.handle(request);
    }

    return this.authorizationService.getToken().pipe(
      mergeMap(token => {
        request = request.clone({
          setHeaders: {
            ContentType: 'application/json',
            Authorization: 'Bearer ' + token
          }
        });
        return next.handle(request);
      })
    );
  }
}
