import {Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError, retry} from 'rxjs/operators';
import {AlertService} from '../services/alert.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private alertService: AlertService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        retry(0),
        catchError((error: HttpErrorResponse) => {
          if (error.error instanceof ErrorEvent) {
            // client side error
          } else {
            console.error(error);
            switch (error.status) {
              case 400:
                switch (error.error.title) {
                  case "BadResponseException":
                    this.alertService.showError(error.error.detail);
                    break;
                  case "ValidationException":
                    this.alertService.showError("Validation Exception!!!");
                    console.log(error.error.errors);
                    break;
                }
                break;
              case 401:
                this.alertService.showError(error.error.title);
                break;
              case 403:
                this.alertService.showError('403, Forbidden!!!');
                break;
              case 404:
                this.alertService.showError(error.error.title);
                break;

              case 500:
                this.alertService.showError(error.error.title);
                console.log(error);
                // TODO Work on methods to display error for 500 error class
                break;
              default:
                this.alertService.showError(error.error.title);
                console.log(error);
                break;
            }
          }
          return throwError('');
        })
      )
  }
}
