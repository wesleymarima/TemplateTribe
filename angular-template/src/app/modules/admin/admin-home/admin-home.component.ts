import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';

@Component({
  selector: 'app-admin-home',
  standalone: false,
  templateUrl: './admin-home.component.html',
  styleUrl: './admin-home.component.scss'
})
export class AdminHomeComponent extends BaseComponent implements OnInit {
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Home');
  }
}
