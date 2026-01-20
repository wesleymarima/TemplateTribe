import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {BaseComponent} from '../../shared/typings/base-component';
import {CostCenterRestService} from '../../shared/services/rest/cost-center.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {AlertService} from '../../shared/services/alert.service';
import {Router} from '@angular/router';
import {RouterConstants} from '../../shared/typings/router-constants';
import {CompanyDTO} from '../../shared/models/company';
import {CostCenterDTO} from '../../shared/models/cost-center';

@Component({
  selector: 'app-admin-cost-center-create',
  standalone: false,
  templateUrl: './admin-cost-center-create.component.html',
  styleUrl: './admin-cost-center-create.component.scss'
})
export class AdminCostCenterCreateComponent extends BaseComponent implements OnInit {

  companies: CompanyDTO[] = [];
  costCenters: CostCenterDTO[] = [];

  form = new FormGroup({
    code: new FormControl('', [Validators.required]),
    name: new FormControl('', [Validators.required]),
    description: new FormControl(''),
    parentCostCenterId: new FormControl<number | null>(null),
    isMandatoryForExpenses: new FormControl(false),
    isMandatoryForProcurement: new FormControl(false),
    isMandatoryForJournalEntries: new FormControl(false),
    companyId: new FormControl<number | null>(null, [Validators.required])
  });

  constructor(
    private costCenterRestService: CostCenterRestService,
    private companyRestService: CompanyRestService,
    private alertService: AlertService,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Cost Center');
    this.loadCompanies();
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe(companies => {
      this.companies = companies;
    });
  }

  onCompanyChange(companyId: number) {
    this.costCenterRestService.getByCompany(companyId).subscribe(costCenters => {
      this.costCenters = costCenters;
    });
  }

  get f() {
    return this.form.controls;
  }

  submit() {
    if (this.form.invalid) {
      this.alertService.showError('Please fill all required fields');
      return;
    }

    const command = {
      code: this.form.value.code!,
      name: this.form.value.name!,
      description: this.form.value.description!,
      parentCostCenterId: this.form.value.parentCostCenterId || null,
      isMandatoryForExpenses: this.form.value.isMandatoryForExpenses!,
      isMandatoryForProcurement: this.form.value.isMandatoryForProcurement!,
      isMandatoryForJournalEntries: this.form.value.isMandatoryForJournalEntries!,
      companyId: this.form.value.companyId!
    };

    this.costCenterRestService.create(command).subscribe({
      next: (id) => {
        this.alertService.showSuccess('Cost center created successfully');
        this.router.navigate([RouterConstants.ADMIN_COST_CENTERS]);
      },
      error: (error) => {
        this.alertService.showError('Failed to create cost center: ' + error.error?.message);
      }
    });
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_COST_CENTERS]);
  }
}

