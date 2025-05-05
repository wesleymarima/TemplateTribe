import {Injectable, signal} from '@angular/core';
import {PaginationMetadata} from '../typings/pagination-meta-data';

@Injectable({providedIn: 'root'})
export class PaginationService {
  pageNumber = signal(1);
  pageSize = signal(15);
  totalItems = signal(0);
  totalPages = signal(0);
  totalCount = signal(0);
  hasPreviousPage = signal(false);
  hasNextPage = signal(false);
  paginationEnabled = signal(false);

  update(meta: PaginationMetadata) {
    this.pageNumber.set(meta.pageNumber);
    this.pageSize.set(meta.pageSize);
    this.totalItems.set(meta.totalItems);
    this.totalPages.set(meta.totalPages);
    this.totalCount.set(meta.totalCount);
    this.hasPreviousPage.set(meta.hasPreviousPage);
    this.hasNextPage.set(meta.hasNextPage);
    this.paginationEnabled.set(true);
  }

  reset() {
    this.pageNumber.set(1);
    this.pageSize.set(17);
    this.totalItems.set(0);
    this.totalPages.set(0);
    this.totalCount.set(0);
    this.hasPreviousPage.set(false);
    this.hasNextPage.set(false);
    this.paginationEnabled.set(false);
  }
}
