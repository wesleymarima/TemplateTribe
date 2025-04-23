import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {AuthService} from '../services/auth.service';
import {Router} from '@angular/router';
import {Observable} from 'rxjs';

// import {environment} from "../../../../environments/environment.prod";

@Injectable()
export class HeaderInterceptor implements HttpInterceptor {
  // baseUrl = environment.apiUrl;

  constructor(private authService: AuthService,
              private router: Router) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.validateToken();
    request = request.clone({
      setHeaders: {
        Accept: 'application/json',
        'Content-type': 'application/json',
        Authorization: `Bearer ${this.authService.getToken()}`,
        'rejectUnauthorized': 'false'
      },
    });
    return next.handle(request);
  }

  validateToken() {
    if (this.authService.isLoggedIn()) {
      if (!this.authService.isTokenValid()) {
        this.authService.logout();
      }
    }

  }
}
