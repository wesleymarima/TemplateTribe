import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountTypeRestService} from '../../shared/services/rest/account-type.rest.service';
import {AccountSubCategoryRestService} from '../../shared/services/rest/account-subcategory.rest.service';
import {AccountTypeDTO} from '../../shared/models/account-type';
import {AccountSubCategoryDTO} from '../../shared/models/account-subcategory';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-account-types-home',
  standalone: false,
  templateUrl: './admin-account-types-home.component.html',
  styleUrl: './admin-account-types-home.component.scss'
})
export class AdminAccountTypesHomeComponent extends BaseComponent implements OnInit {

  accountTypes: AccountTypeDTO[] = [];
  subCategories: AccountSubCategoryDTO[] = [];
  selectedSubCategoryId?: number;
  displayedColumns: string[] = ['code', 'name', 'accountSubCategoryName', 'normalBalance', 'displayOrder', 'isActive', 'actions'];

  constructor(
    private accountTypeRestService: AccountTypeRestService,
    private accountSubCategoryRestService: AccountSubCategoryRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Account Types');
    this.loadSubCategories();
    this.getAll();
  }

  loadSubCategories() {
    this.accountSubCategoryRestService.getAll().subscribe(subCategories => {
      this.subCategories = subCategories.filter(sc => sc.isActive);
    });
  }

  getAll() {
    this.accountTypeRestService.getAll(this.selectedSubCategoryId).subscribe(response => {
      this.accountTypes = response;
    });
  }

  onSubCategoryChange() {
    this.getAll();
  }

  toggleStatus(id: number, currentStatus: boolean) {
    const newStatus = !currentStatus;
    this.accountTypeRestService.toggleStatus(id, newStatus).subscribe(() => {
      this.alertService.showSuccess('Status updated successfully');
      this.getAll();
    });
  }

  protected readonly RouterConstants = RouterConstants;
}
