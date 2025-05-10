import {BaseResponse} from '../models/base-response';
import {inject} from '@angular/core';
import {BaseSharedService} from '../services/base.shared.service';

export abstract class BaseComponent {
  pageNumber: number = 1;
  totalPages: number = 0;
  totalCount: number = 0;
  pageSize: number = 20;
  hasPreviousPage: boolean = false;
  hasNextPage: boolean = false;

  protected baseSharedService = inject(BaseSharedService);

  updateData(baseResponse: BaseResponse<any>) {
    this.pageNumber = baseResponse.pageNumber;
    this.totalPages = baseResponse.totalPages;
    this.totalCount = baseResponse.totalCount;
    this.pageSize = baseResponse.items.length;
    this.hasPreviousPage = baseResponse.hasPreviousPage;
    this.hasNextPage = baseResponse.hasNextPage;
  }

  updatePageName(pageName: string) {
    this.baseSharedService.updateTitle(pageName);
  }
}
