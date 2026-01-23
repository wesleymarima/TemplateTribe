import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountRestService} from '../../shared/services/rest/account.rest.service';
import {AccountDTO} from '../../shared/models/account';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {CompanyDTO} from '../../shared/models/company';

@Component({
  selector: 'app-admin-accounts-home',
  standalone: false,
  templateUrl: './admin-accounts-home.component.html',
  styleUrl: './admin-accounts-home.component.scss'
})
export class AdminAccountsHomeComponent extends BaseComponent implements OnInit {

  accounts: AccountDTO[] = [];
  companies: CompanyDTO[] = [];
  selectedCompanyId?: number;
  displayedColumns: string[] = ['accountCode', 'accountName', 'accountTypeName', 'level', 'currentBalance', 'isActive', 'actions'];

  constructor(
    private accountRestService: AccountRestService,
    private companyRestService: CompanyRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Chart of Accounts');
    this.loadCompanies();
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe(companies => {
      this.companies = companies;
      if (companies.length > 0) {
        this.selectedCompanyId = companies[0].id;
        this.getAll();
      }
    });
  }

  getAll() {
    this.accountRestService.getAll(this.selectedCompanyId).subscribe(response => {
      this.accounts = response;
    });
  }

  onCompanyChange() {
    this.getAll();
  }

  deleteAccount(id: number) {
    if (confirm('Are you sure you want to delete this account?')) {
      this.accountRestService.delete(id).subscribe(() => {
        this.alertService.showSuccess('Account deleted successfully');
        this.getAll();
      });
    }
  }

  toggleStatus(id: number, currentStatus: boolean) {
    const newStatus = !currentStatus;
    this.accountRestService.toggleStatus(id, newStatus).subscribe(() => {
      this.alertService.showSuccess('Status updated successfully');
      this.getAll();
    });
  }

  protected readonly RouterConstants = RouterConstants;
}
