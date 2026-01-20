import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {FinancialPeriodRestService} from '../../shared/services/rest/financial-period.rest.service';
import {FinancialPeriodDTO, PeriodStatus} from '../../shared/models/financial-period';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {CompanyDTO} from '../../shared/models/company';

@Component({
  selector: 'app-admin-financial-periods-home',
  standalone: false,
  templateUrl: './admin-financial-periods-home.component.html',
  styleUrl: './admin-financial-periods-home.component.scss'
})
export class AdminFinancialPeriodsHomeComponent extends BaseComponent implements OnInit {

  financialPeriods: FinancialPeriodDTO[] = [];
  companies: CompanyDTO[] = [];
  selectedCompanyId: number | null = null;
  displayedColumns: string[] = ['id', 'name', 'fiscalYear', 'periodNumber', 'startDate', 'endDate', 'status', 'actions'];

  constructor(
    private financialPeriodRestService: FinancialPeriodRestService,
    private companyRestService: CompanyRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Financial Periods');
    this.loadCompanies();
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe(companies => {
      this.companies = companies;
    });
  }

  onCompanyChange() {
    if (this.selectedCompanyId) {
      this.financialPeriodRestService.getByCompany(this.selectedCompanyId).subscribe(periods => {
        this.financialPeriods = periods;
      });
    }
  }

  closePeriod(id: number) {
    if (confirm('Are you sure you want to close this financial period?')) {
      this.financialPeriodRestService.close(id).subscribe(() => {
        this.alertService.showSuccess('Financial period closed successfully');
        this.onCompanyChange();
      }, error => {
        this.alertService.showError('Failed to close period: ' + error.error?.message);
      });
    }
  }

  reopenPeriod(id: number) {
    const reason = prompt('Please provide a reason for reopening this period:');
    if (reason) {
      const command = {id, reopenReason: reason};
      this.financialPeriodRestService.reopen(id, command).subscribe(() => {
        this.alertService.showSuccess('Financial period reopened successfully');
        this.onCompanyChange();
      }, error => {
        this.alertService.showError('Failed to reopen period: ' + error.error?.message);
      });
    }
  }

  deletePeriod(id: number) {
    if (confirm('Are you sure you want to delete this financial period?')) {
      this.financialPeriodRestService.delete(id).subscribe(() => {
        this.alertService.showSuccess('Financial period deleted successfully');
        this.onCompanyChange();
      }, error => {
        this.alertService.showError('Failed to delete period: ' + error.error?.message);
      });
    }
  }

  protected readonly PeriodStatus = PeriodStatus;
  protected readonly RouterConstants = RouterConstants;

}

