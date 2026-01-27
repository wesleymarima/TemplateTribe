import {Component, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {BaseComponent} from '../../shared/typings/base-component';
import {JournalEntryRestService} from '../../shared/services/rest/journal-entry.rest.service';
import {AccountRestService} from '../../shared/services/rest/account.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {CostCenterRestService} from '../../shared/services/rest/cost-center.rest.service';
import {DepartmentRestService} from '../../shared/services/rest/department.rest.service';
import {BranchRestService} from '../../shared/services/rest/branch.rest.service';
import {AccountDTO} from '../../shared/models/account';
import {CompanyDTO} from '../../shared/models/company';
import {CostCenterDTO} from '../../shared/models/cost-center';
import {DepartmentDTO} from '../../shared/models/department';
import {BranchDTO} from '../../shared/models/branch';
import {JournalEntryType} from '../../shared/models/journal-entry';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-journal-entry-create',
  standalone: false,
  templateUrl: './admin-journal-entry-create.component.html',
  styleUrl: './admin-journal-entry-create.component.scss'
})
export class AdminJournalEntryCreateComponent extends BaseComponent implements OnInit {

  form: FormGroup;
  accounts: AccountDTO[] = [];
  companies: CompanyDTO[] = [];
  costCenters: CostCenterDTO[] = [];
  departments: DepartmentDTO[] = [];
  branches: BranchDTO[] = [];
  journalEntryTypes = [
    {value: JournalEntryType.Manual, name: 'Manual'},
    {value: JournalEntryType.Opening, name: 'Opening'},
    {value: JournalEntryType.Adjustment, name: 'Adjustment'},
    {value: JournalEntryType.Reversal, name: 'Reversal'},
    {value: JournalEntryType.Closing, name: 'Closing'},
    {value: JournalEntryType.Recurring, name: 'Recurring'}
  ];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private journalEntryRestService: JournalEntryRestService,
    private accountRestService: AccountRestService,
    private companyRestService: CompanyRestService,
    private costCenterRestService: CostCenterRestService,
    private departmentRestService: DepartmentRestService,
    private branchRestService: BranchRestService,
    private alertService: AlertService
  ) {
    super();
    this.form = this.fb.group({
      transactionDate: [new Date().toISOString().substring(0, 10), Validators.required],
      description: ['', Validators.required],
      referenceNumber: [''],
      entryType: [JournalEntryType.Manual, Validators.required],
      lines: this.fb.array([])
    });
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Journal Entry');
    this.loadCompanies();
    this.loadCostCenters();
    this.loadDepartments();
    this.loadBranches();

    // Check if accountId is passed as query param
    const accountId = this.route.snapshot.queryParams['accountId'];
    if (accountId) {
      this.addLine();
      // We'll set the account after loading accounts
    } else {
      this.addLine();
      this.addLine();
    }
  }

  get lines(): FormArray {
    return this.form.get('lines') as FormArray;
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe(companies => {
      this.companies = companies.filter(c => c.isActive);
      if (companies.length > 0) {
        this.loadAccounts(companies[0].id);
      }
    });
  }

  loadAccounts(companyId: number) {
    this.accountRestService.getAll(companyId).subscribe(accounts => {
      this.accounts = accounts.filter(a => a.isActive && a.allowDirectPosting);

      // Set account if passed via query param
      const accountId = this.route.snapshot.queryParams['accountId'];
      if (accountId && this.lines.length > 0) {
        this.lines.at(0).patchValue({accountId: Number(accountId)});
      }
    });
  }

  loadCostCenters() {
    this.costCenterRestService.getAll().subscribe(centers => {
      this.costCenters = centers.filter(c => c.isActive);
    });
  }

  loadDepartments() {
    this.departmentRestService.getAll().subscribe(depts => {
      this.departments = depts.filter(d => d.isActive);
    });
  }

  loadBranches() {
    this.branchRestService.getAll().subscribe(branches => {
      this.branches = branches.filter(b => b.isActive);
    });
  }

  addLine() {
    const lineGroup = this.fb.group({
      lineNumber: [this.lines.length + 1],
      accountId: [null, Validators.required],
      description: ['', Validators.required],
      debitAmount: [0, [Validators.required, Validators.min(0)]],
      creditAmount: [0, [Validators.required, Validators.min(0)]],
      costCenterId: [null],
      departmentId: [null],
      branchId: [null],
      analysisCode: [''],
      memo: ['']
    });
    this.lines.push(lineGroup);
  }

  removeLine(index: number) {
    if (this.lines.length > 2) {
      this.lines.removeAt(index);
      // Update line numbers
      this.lines.controls.forEach((control, i) => {
        control.patchValue({lineNumber: i + 1});
      });
    } else {
      this.alertService.showError('Journal entry must have at least 2 lines');
    }
  }

  getTotalDebit(): number {
    return this.lines.controls.reduce((sum, control) => {
      return sum + (Number(control.value.debitAmount) || 0);
    }, 0);
  }

  getTotalCredit(): number {
    return this.lines.controls.reduce((sum, control) => {
      return sum + (Number(control.value.creditAmount) || 0);
    }, 0);
  }

  isBalanced(): boolean {
    const totalDebit = this.getTotalDebit();
    const totalCredit = this.getTotalCredit();
    return Math.abs(totalDebit - totalCredit) < 0.01 && totalDebit > 0;
  }

  onSubmit() {
    if (this.form.valid) {
      if (!this.isBalanced()) {
        this.alertService.showError('Journal entry must be balanced (Total Debit must equal Total Credit)');
        return;
      }

      const command = this.form.value;
      this.journalEntryRestService.create(command).subscribe({
        next: (id) => {
          this.alertService.showSuccess('Journal entry created successfully');
          this.router.navigate([RouterConstants.ADMIN_JOURNAL_ENTRY_VIEW, id]);
        },
        error: () => {
          this.alertService.showError('Failed to create journal entry');
        }
      });
    }
  }

  goBack() {
    this.router.navigate([RouterConstants.ADMIN_JOURNAL_ENTRIES]);
  }

  protected readonly RouterConstants = RouterConstants;
}
