import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {ExchangeRateRestService} from '../../shared/services/rest/exchange-rate.rest.service';
import {ExchangeRateDTO} from '../../shared/models/exchange-rate';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';
import {CurrencyRestService} from '../../shared/services/rest/currency.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {CurrencyDTO} from '../../shared/models/currency';
import {CompanyDTO} from '../../shared/models/company';

@Component({
  selector: 'app-admin-exchange-rates-home',
  standalone: false,
  templateUrl: './admin-exchange-rates-home.component.html',
  styleUrl: './admin-exchange-rates-home.component.scss'
})
export class AdminExchangeRatesHomeComponent extends BaseComponent implements OnInit {

  exchangeRates: ExchangeRateDTO[] = [];
  currencies: CurrencyDTO[] = [];
  companies: CompanyDTO[] = [];
  selectedCurrencyId: number | null = null;
  selectedCompanyId: number | null = null;

  displayedColumns: string[] = ['id', 'currencyCode', 'toCurrencyCode', 'rate', 'effectiveDate', 'endDate', 'isActive', 'actions'];

  constructor(
    private exchangeRateRestService: ExchangeRateRestService,
    private currencyRestService: CurrencyRestService,
    private companyRestService: CompanyRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Exchange Rates');
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

  onFilterChange() {
    if (this.selectedCurrencyId && this.selectedCompanyId) {
      this.exchangeRateRestService.getByCurrency(this.selectedCurrencyId, this.selectedCompanyId)
        .subscribe(rates => {
          this.exchangeRates = rates;
        });
    }
  }

  deleteExchangeRate(id: number) {
    if (confirm('Are you sure you want to delete this exchange rate?')) {
      this.exchangeRateRestService.delete(id).subscribe(() => {
        this.alertService.showSuccess('Exchange rate deleted successfully');
        this.onFilterChange();
      }, error => {
        this.alertService.showError('Failed to delete exchange rate: ' + error.error?.message);
      });
    }
  }

  protected readonly RouterConstants = RouterConstants;
}

