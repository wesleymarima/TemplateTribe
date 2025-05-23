import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest,} from '@angular/common/http';
import {Observable} from 'rxjs';
import {finalize} from 'rxjs/operators';
import {LoaderService} from '../services/loader.service';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {

  constructor(private loaderService: LoaderService) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const skipInterceptor = request.params.get('usrRecommendations') === 'true';
    if (skipInterceptor) {
      return next.handle(request); // Pass request directly to the next handler
    }

    this.loaderService.show();

    return next.handle(request).pipe(
      finalize(() => this.loaderService.hide()),
    );
  }
}
