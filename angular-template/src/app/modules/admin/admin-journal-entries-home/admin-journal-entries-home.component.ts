import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {JournalEntryRestService} from '../../shared/services/rest/journal-entry.rest.service';
import {FinancialPeriodRestService} from '../../shared/services/rest/financial-period.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {JournalEntryDTO} from '../../shared/models/journal-entry';
import {FinancialPeriodDTO} from '../../shared/models/financial-period';
import {CompanyDTO} from '../../shared/models/company';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-journal-entries-home',
  standalone: false,
  templateUrl: './admin-journal-entries-home.component.html',
  styleUrl: './admin-journal-entries-home.component.scss'
})
export class AdminJournalEntriesHomeComponent extends BaseComponent implements OnInit {

  journalEntries: JournalEntryDTO[] = [];
  financialPeriods: FinancialPeriodDTO[] = [];
  companies: CompanyDTO[] = [];
  selectedFinancialPeriodId?: number;
  selectedCompanyId?: number;
  displayedColumns: string[] = ['journalNumber', 'transactionDate', 'description', 'referenceNumber', 'entryType', 'status', 'totalDebit', 'totalCredit', 'actions'];

  constructor(
    private journalEntryRestService: JournalEntryRestService,
    private financialPeriodRestService: FinancialPeriodRestService,
    private companyRestService: CompanyRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Journal Entries');
    this.loadCompanies();
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe((companies: CompanyDTO[]) => {
      this.companies = companies.filter(c => c.isActive);
      if (companies.length > 0) {
        this.selectedCompanyId = companies[0].id;
        this.loadFinancialPeriods();
      }
    });
  }

  loadFinancialPeriods() {
    if (this.selectedCompanyId) {
      this.financialPeriodRestService.getByCompany(this.selectedCompanyId).subscribe((periods: FinancialPeriodDTO[]) => {
        this.financialPeriods = periods;
        this.getAll();
      });
    }
  }

  getAll() {
    this.journalEntryRestService.getAll(this.selectedFinancialPeriodId).subscribe(response => {
      this.journalEntries = response;
    });
  }

  onFinancialPeriodChange() {
    this.getAll();
  }

  onCompanyChange() {
    this.selectedFinancialPeriodId = undefined;
    this.loadFinancialPeriods();
  }

  postJournalEntry(id: number) {
    if (confirm('Are you sure you want to post this journal entry? This action will update account balances.')) {
      this.journalEntryRestService.post(id).subscribe({
        next: () => {
          this.alertService.showSuccess('Journal entry posted successfully');
          this.getAll();
        },
        error: () => {
          this.alertService.showError('Failed to post journal entry');
        }
      });
    }
  }

  deleteJournalEntry(id: number) {
    if (confirm('Are you sure you want to delete this journal entry?')) {
      this.journalEntryRestService.delete(id).subscribe({
        next: () => {
          this.alertService.showSuccess('Journal entry deleted successfully');
          this.getAll();
        },
        error: () => {
          this.alertService.showError('Failed to delete journal entry');
        }
      });
    }
  }

  protected readonly RouterConstants = RouterConstants;
}
