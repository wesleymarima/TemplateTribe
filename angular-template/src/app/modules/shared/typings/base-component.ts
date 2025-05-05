export abstract class BaseComponent {
  pageNumber: number = 1;
  totalPages: number = 0;
  totalCount: number = 0;
  pageSize: number = 10;
  hasPreviousPage: boolean = false;
  hasNextPage: boolean = false;
}
