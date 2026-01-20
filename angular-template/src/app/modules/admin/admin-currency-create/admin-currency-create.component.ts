import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {BaseComponent} from '../../shared/typings/base-component';
import {CurrencyRestService} from '../../shared/services/rest/currency.rest.service';
import {AlertService} from '../../shared/services/alert.service';
import {Router} from '@angular/router';
import {RouterConstants} from '../../shared/typings/router-constants';

@Component({
  selector: 'app-admin-currency-create',
  standalone: false,
  templateUrl: './admin-currency-create.component.html',
  styleUrl: './admin-currency-create.component.scss'
})
export class AdminCurrencyCreateComponent extends BaseComponent implements OnInit {

  form = new FormGroup({
    code: new FormControl('', [Validators.required, Validators.maxLength(3)]),
    name: new FormControl('', [Validators.required]),
    symbol: new FormControl('', [Validators.required]),
    decimalPlaces: new FormControl(2, [Validators.required, Validators.min(0)])
  });

  constructor(
    private currencyRestService: CurrencyRestService,
    private alertService: AlertService,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Currency');
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
      symbol: this.form.value.symbol!,
      decimalPlaces: this.form.value.decimalPlaces!
    };

    this.currencyRestService.create(command).subscribe({
      next: (id) => {
        this.alertService.showSuccess('Currency created successfully');
        this.router.navigate([RouterConstants.ADMIN_CURRENCIES]);
      },
      error: (error) => {
        this.alertService.showError('Failed to create currency: ' + error.error?.message);
      }
    });
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_CURRENCIES]);
  }
}

