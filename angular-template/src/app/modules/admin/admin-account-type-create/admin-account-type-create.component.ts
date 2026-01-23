import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountTypeRestService} from '../../shared/services/rest/account-type.rest.service';
import {AccountSubCategoryRestService} from '../../shared/services/rest/account-subcategory.rest.service';
import {AccountSubCategoryDTO} from '../../shared/models/account-subcategory';
import {NormalBalance} from '../../shared/models/account-type';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-account-type-create',
  standalone: false,
  templateUrl: './admin-account-type-create.component.html',
  styleUrl: './admin-account-type-create.component.scss'
})
export class AdminAccountTypeCreateComponent extends BaseComponent implements OnInit {

  form: FormGroup;
  subCategories: AccountSubCategoryDTO[] = [];
  normalBalances = [
    {value: NormalBalance.Debit, label: 'Debit'},
    {value: NormalBalance.Credit, label: 'Credit'}
  ];

  constructor(
    private fb: FormBuilder,
    private accountTypeRestService: AccountTypeRestService,
    private accountSubCategoryRestService: AccountSubCategoryRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.form = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      description: [''],
      accountSubCategoryId: ['', Validators.required],
      normalBalance: [NormalBalance.Debit, Validators.required],
      displayOrder: [1, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Account Type');
    this.loadSubCategories();
  }

  loadSubCategories() {
    this.accountSubCategoryRestService.getAll().subscribe(subCategories => {
      this.subCategories = subCategories.filter(sc => sc.isActive);
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.accountTypeRestService.create(this.form.value).subscribe({
        next: () => {
          this.alertService.showSuccess('Account type created successfully');
          this.router.navigate([RouterConstants.ADMIN_ACCOUNT_TYPES]);
        },
        error: () => {
          this.alertService.showError('Failed to create account type');
        }
      });
    }
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_ACCOUNT_TYPES]);
  }

  protected readonly RouterConstants = RouterConstants;
}
