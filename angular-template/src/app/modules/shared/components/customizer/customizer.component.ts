import {Component, EventEmitter, Output, ViewEncapsulation,} from '@angular/core';
import {AppSettings} from '../../../app/app.config';
import {CoreService} from '../../services/core.service';

@Component({
  selector: 'app-customizer',
  standalone: false,
  templateUrl: './customizer.component.html',
  styleUrls: ['./customizer.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class CustomizerComponent {

  @Output() optionsChange = new EventEmitter<AppSettings>();

  constructor(private settings: CoreService) {
    this.options = this.settings.getOptions();
  }

  options: AppSettings;

  setDark() {
    this.optionsChange.emit(this.options);
  }

  setColor() {
    this.optionsChange.emit(this.options);
  }

  setDir() {
    this.optionsChange.emit(this.options);
  }

  setSidebar() {
    this.optionsChange.emit(this.options);
  }
}
