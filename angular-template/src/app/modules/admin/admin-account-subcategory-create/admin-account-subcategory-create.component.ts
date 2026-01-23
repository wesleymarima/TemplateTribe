import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountSubCategoryRestService} from '../../shared/services/rest/account-subcategory.rest.service';
import {AccountCategoryRestService} from '../../shared/services/rest/account-category.rest.service';
import {AccountCategoryDTO} from '../../shared/models/account-category';
import {NormalBalance} from '../../shared/models/account-subcategory';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-account-subcategory-create',
  standalone: false,
  templateUrl: './admin-account-subcategory-create.component.html',
  styleUrl: './admin-account-subcategory-create.component.scss'
})
export class AdminAccountSubcategoryCreateComponent extends BaseComponent implements OnInit {

  form: FormGroup;
  categories: AccountCategoryDTO[] = [];
  normalBalances = [
    {value: NormalBalance.Debit, label: 'Debit'},
    {value: NormalBalance.Credit, label: 'Credit'}
  ];

  constructor(
    private fb: FormBuilder,
    private accountSubCategoryRestService: AccountSubCategoryRestService,
    private accountCategoryRestService: AccountCategoryRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.form = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      description: [''],
      accountCategoryId: ['', Validators.required],
      normalBalance: [NormalBalance.Debit, Validators.required],
      displayOrder: [1, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Account Sub-Category');
    this.loadCategories();
  }

  loadCategories() {
    this.accountCategoryRestService.getAll().subscribe(categories => {
      this.categories = categories.filter(c => c.isActive);
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.accountSubCategoryRestService.create(this.form.value).subscribe({
        next: () => {
          this.alertService.showSuccess('Account sub-category created successfully');
          this.router.navigate([RouterConstants.ADMIN_ACCOUNT_SUBCATEGORIES]);
        },
        error: () => {
          this.alertService.showError('Failed to create account sub-category');
        }
      });
    }
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_ACCOUNT_SUBCATEGORIES]);
  }

  protected readonly RouterConstants = RouterConstants;
}
