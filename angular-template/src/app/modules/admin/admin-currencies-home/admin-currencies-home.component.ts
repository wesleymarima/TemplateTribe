import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {CurrencyRestService} from '../../shared/services/rest/currency.rest.service';
import {CurrencyDTO} from '../../shared/models/currency';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-currencies-home',
  standalone: false,
  templateUrl: './admin-currencies-home.component.html',
  styleUrl: './admin-currencies-home.component.scss'
})
export class AdminCurrenciesHomeComponent extends BaseComponent implements OnInit {

  currencies: CurrencyDTO[] = [];
  displayedColumns: string[] = ['id', 'code', 'name', 'symbol', 'decimalPlaces', 'isActive', 'actions'];

  constructor(
    private currencyRestService: CurrencyRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Currencies');
    this.getAll();
  }

  getAll() {
    this.currencyRestService.getAll().subscribe(response => {
      this.currencies = response;
    });
  }

  deleteCurrency(id: number) {
    if (confirm('Are you sure you want to delete this currency?')) {
      this.currencyRestService.delete(id).subscribe(() => {
        this.alertService.showSuccess('Currency deleted successfully');
        this.getAll();
      }, error => {
        this.alertService.showError('Failed to delete currency: ' + error.error?.message);
      });
    }
  }

  toggleStatus(currency: CurrencyDTO) {
    const updateCommand = {
      id: currency.id,
      code: currency.code,
      name: currency.name,
      symbol: currency.symbol,
      decimalPlaces: currency.decimalPlaces,
      isActive: !currency.isActive
    };

    this.currencyRestService.update(currency.id, updateCommand).subscribe(() => {
      this.alertService.showSuccess('Currency status updated successfully');
      this.getAll();
    }, error => {
      this.alertService.showError('Failed to update currency: ' + error.error?.message);
    });
  }

  protected readonly RouterConstants = RouterConstants;
}

