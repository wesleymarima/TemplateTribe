import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountCategoryRestService} from '../../shared/services/rest/account-category.rest.service';
import {CategoryType, NormalBalance} from '../../shared/models/account-category';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-account-category-create',
  standalone: false,
  templateUrl: './admin-account-category-create.component.html',
  styleUrl: './admin-account-category-create.component.scss'
})
export class AdminAccountCategoryCreateComponent extends BaseComponent implements OnInit {

  form: FormGroup;
  categoryTypes = [
    {value: CategoryType.Asset, label: 'Asset'},
    {value: CategoryType.Liability, label: 'Liability'},
    {value: CategoryType.Equity, label: 'Equity'},
    {value: CategoryType.Revenue, label: 'Revenue'},
    {value: CategoryType.Expense, label: 'Expense'}
  ];
  normalBalances = [
    {value: NormalBalance.Debit, label: 'Debit'},
    {value: NormalBalance.Credit, label: 'Credit'}
  ];

  constructor(
    private fb: FormBuilder,
    private accountCategoryRestService: AccountCategoryRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.form = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      description: [''],
      type: [CategoryType.Asset, Validators.required],
      normalBalance: [NormalBalance.Debit, Validators.required],
      displayOrder: [1, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Account Category');
  }

  onSubmit() {
    if (this.form.valid) {
      this.accountCategoryRestService.create(this.form.value).subscribe({
        next: () => {
          this.alertService.showSuccess('Account category created successfully');
          this.router.navigate([RouterConstants.ADMIN_ACCOUNT_CATEGORIES]);
        },
        error: (error) => {
          this.alertService.showError('Failed to create account category');
        }
      });
    }
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_ACCOUNT_CATEGORIES]);
  }

  protected readonly RouterConstants = RouterConstants;
}
