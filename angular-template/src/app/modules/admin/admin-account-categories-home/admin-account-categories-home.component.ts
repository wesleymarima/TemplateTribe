import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountCategoryRestService} from '../../shared/services/rest/account-category.rest.service';
import {AccountCategoryDTO} from '../../shared/models/account-category';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-account-categories-home',
  standalone: false,
  templateUrl: './admin-account-categories-home.component.html',
  styleUrl: './admin-account-categories-home.component.scss'
})
export class AdminAccountCategoriesHomeComponent extends BaseComponent implements OnInit {

  categories: AccountCategoryDTO[] = [];
  displayedColumns: string[] = ['code', 'name', 'type', 'normalBalance', 'displayOrder', 'isActive', 'actions'];

  constructor(
    private accountCategoryRestService: AccountCategoryRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Account Categories');
    this.getAll();
  }

  getAll() {
    this.accountCategoryRestService.getAll().subscribe(response => {
      this.categories = response;
    });
  }

  toggleStatus(id: number, currentStatus: boolean) {
    const newStatus = !currentStatus;
    this.accountCategoryRestService.toggleStatus(id, newStatus).subscribe(() => {
      this.alertService.showSuccess('Status updated successfully');
      this.getAll();
    });
  }

  protected readonly RouterConstants = RouterConstants;
}
