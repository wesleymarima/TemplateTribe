import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountSubCategoryRestService} from '../../shared/services/rest/account-subcategory.rest.service';
import {AccountCategoryRestService} from '../../shared/services/rest/account-category.rest.service';
import {AccountSubCategoryDTO} from '../../shared/models/account-subcategory';
import {AccountCategoryDTO} from '../../shared/models/account-category';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-account-subcategories-home',
  standalone: false,
  templateUrl: './admin-account-subcategories-home.component.html',
  styleUrl: './admin-account-subcategories-home.component.scss'
})
export class AdminAccountSubcategoriesHomeComponent extends BaseComponent implements OnInit {

  subcategories: AccountSubCategoryDTO[] = [];
  categories: AccountCategoryDTO[] = [];
  selectedCategoryId?: number;
  displayedColumns: string[] = ['code', 'name', 'accountCategoryName', 'normalBalance', 'displayOrder', 'isActive', 'actions'];

  constructor(
    private accountSubCategoryRestService: AccountSubCategoryRestService,
    private accountCategoryRestService: AccountCategoryRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Account Sub-Categories');
    this.loadCategories();
    this.getAll();
  }

  loadCategories() {
    this.accountCategoryRestService.getAll().subscribe(categories => {
      this.categories = categories.filter(c => c.isActive);
    });
  }

  getAll() {
    this.accountSubCategoryRestService.getAll(this.selectedCategoryId).subscribe(response => {
      this.subcategories = response;
    });
  }

  onCategoryChange() {
    this.getAll();
  }

  toggleStatus(id: number, currentStatus: boolean) {
    const newStatus = !currentStatus;
    this.accountSubCategoryRestService.toggleStatus(id, newStatus).subscribe(() => {
      this.alertService.showSuccess('Status updated successfully');
      this.getAll();
    });
  }

  protected readonly RouterConstants = RouterConstants;
}
