import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {BaseComponent} from '../../shared/typings/base-component';
import {ExchangeRateRestService} from '../../shared/services/rest/exchange-rate.rest.service';
import {CurrencyRestService} from '../../shared/services/rest/currency.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {AlertService} from '../../shared/services/alert.service';
import {Router} from '@angular/router';
import {RouterConstants} from '../../shared/typings/router-constants';
import {CurrencyDTO} from '../../shared/models/currency';
import {CompanyDTO} from '../../shared/models/company';

@Component({
  selector: 'app-admin-exchange-rate-create',
  standalone: false,
  templateUrl: './admin-exchange-rate-create.component.html',
  styleUrl: './admin-exchange-rate-create.component.scss'
})
export class AdminExchangeRateCreateComponent extends BaseComponent implements OnInit {

  currencies: CurrencyDTO[] = [];
  companies: CompanyDTO[] = [];

  form = new FormGroup({
    currencyId: new FormControl<number | null>(null, [Validators.required]),
    toCurrencyCode: new FormControl('', [Validators.required]),
    rate: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    effectiveDate: new FormControl('', [Validators.required]),
    endDate: new FormControl<string | null>(null),
    companyId: new FormControl<number | null>(null, [Validators.required])
  });

  constructor(
    private exchangeRateRestService: ExchangeRateRestService,
    private currencyRestService: CurrencyRestService,
    private companyRestService: CompanyRestService,
    private alertService: AlertService,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Exchange Rate');
    this.loadCurrencies();
    this.loadCompanies();
  }

  loadCurrencies() {
    this.currencyRestService.getActive().subscribe(currencies => {
      this.currencies = currencies;
    });
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe(companies => {
      this.companies = companies;
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
      currencyId: this.form.value.currencyId!,
      toCurrencyCode: this.form.value.toCurrencyCode!,
      rate: this.form.value.rate!,
      effectiveDate: this.form.value.effectiveDate!,
      endDate: this.form.value.endDate || null,
      companyId: this.form.value.companyId!
    };

    this.exchangeRateRestService.create(command).subscribe({
      next: (id) => {
        this.alertService.showSuccess('Exchange rate created successfully');
        this.router.navigate([RouterConstants.ADMIN_EXCHANGE_RATES]);
      },
      error: (error) => {
        this.alertService.showError('Failed to create exchange rate: ' + error.error?.message);
      }
    });
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_EXCHANGE_RATES]);
  }
}

