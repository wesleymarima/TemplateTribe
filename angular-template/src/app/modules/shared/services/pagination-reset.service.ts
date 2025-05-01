import {Injectable} from '@angular/core';
import {NavigationEnd, Router} from '@angular/router';
import {PaginationService} from './pagination.service';
import {filter} from 'rxjs';

@Injectable({providedIn: 'root'})
export class PaginationResetService {
  constructor(private router: Router, private pagination: PaginationService) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.pagination.reset();
      });
  }
}
