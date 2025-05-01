import {PaginationService} from '../services/pagination.service';
import {Injectable} from '@angular/core';

@Injectable()
export abstract class BaseComponent {
  protected constructor(protected paginationService: PaginationService) {
  }
}
