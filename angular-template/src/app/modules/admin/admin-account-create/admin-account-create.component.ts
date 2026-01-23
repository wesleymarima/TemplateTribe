import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountRestService} from '../../shared/services/rest/account.rest.service';
import {AccountTypeRestService} from '../../shared/services/rest/account-type.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';
import {AccountTypeDTO} from '../../shared/models/account-type';
import {CompanyDTO} from '../../shared/models/company';
import {AccountDTO} from '../../shared/models/account';

@Component({
  selector: 'app-admin-account-create',
  standalone: false,
  templateUrl: './admin-account-create.component.html',
  styleUrl: './admin-account-create.component.scss'
})
export class AdminAccountCreateComponent extends BaseComponent implements OnInit {

  form: FormGroup;
  accountTypes: AccountTypeDTO[] = [];
  companies: CompanyDTO[] = [];
  parentAccounts: AccountDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private accountRestService: AccountRestService,
    private accountTypeRestService: AccountTypeRestService,
    private companyRestService: CompanyRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.form = this.fb.group({
      accountCode: ['', Validators.required],
      accountName: ['', Validators.required],
      description: [''],
      accountTypeId: ['', Validators.required],
      parentAccountId: [null],
      companyId: ['', Validators.required],
      currencyId: [null],
      level: [1, [Validators.required, Validators.min(1)]],
      allowDirectPosting: [true],
      requiresCostCenter: [false],
      requiresDepartment: [false],
      requiresBranch: [false],
      openingBalance: [0],
      openingBalanceDate: [null]
    });
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Account');
    this.loadAccountTypes();
    this.loadCompanies();
  }

  loadAccountTypes() {
    this.accountTypeRestService.getAll().subscribe(types => {
      this.accountTypes = types.filter(t => t.isActive);
    });
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe(companies => {
      this.companies = companies.filter(c => c.isActive);
      if (companies.length > 0) {
        this.form.patchValue({companyId: companies[0].id});
        this.loadParentAccounts(companies[0].id);
      }
    });
  }

  loadParentAccounts(companyId: number) {
    this.accountRestService.getAll(companyId).subscribe(accounts => {
      this.parentAccounts = accounts.filter(a => a.isActive);
    });
  }

  onCompanyChange() {
    const companyId = this.form.get('companyId')?.value;
    if (companyId) {
      this.loadParentAccounts(companyId);
    }
  }

  onSubmit() {
    if (this.form.valid) {
      this.accountRestService.create(this.form.value).subscribe({
        next: () => {
          this.alertService.showSuccess('Account created successfully');
          this.router.navigate([RouterConstants.ADMIN_ACCOUNTS]);
        },
        error: (error) => {
          this.alertService.showError('Failed to create account');
        }
      });
    }
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_ACCOUNTS]);
  }

  protected readonly RouterConstants = RouterConstants;
}
