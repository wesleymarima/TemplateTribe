import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-shared-paginate',
  standalone: false,
  templateUrl: './shared-paginate.component.html',
  styleUrl: './shared-paginate.component.scss'
})
export class SharedPaginateComponent {
  @Input() pageNumber: number = 0;
  @Input() totalPages: number = 0;
  @Input() totalCount: number = 0;
  @Input() hasPreviousPage: boolean = false;
  @Input() hasNextPage: boolean = false;
  @Output() pageChange = new EventEmitter<{ pageNumber: number; pageSize: number }>();


  onPageChange(event: any): void {
    this.pageChange.emit({
      pageNumber: event.pageIndex + 1,
      pageSize: event.pageSize
    });
  }

}
