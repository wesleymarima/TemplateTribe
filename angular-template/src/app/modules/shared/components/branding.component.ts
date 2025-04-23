import {Component} from '@angular/core';
import {CoreService} from '../services/core.service';
import {AppSettings} from '../../app/app.config';

@Component({
  selector: 'app-branding',
  standalone: false,
  template: `
    <div class="branding">
      <a href="/" *ngIf="options.theme === 'light'">
        <img
          src="./assets/images/logos/dark-logo.svg"
          class="align-middle m-2"
          alt="logo"
        />
      </a>
      <a href="/" *ngIf="options.theme === 'dark'">
        <img
          src="./assets/images/logos/light-logo.svg"
          class="align-middle m-2"
          alt="logo"
        />
      </a>
    </div>
  `,
})
export class BrandingComponent {
  options: AppSettings;

  constructor(private settings: CoreService) {
    this.options = this.settings.getOptions();
  }
}
