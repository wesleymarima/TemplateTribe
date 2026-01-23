import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {RouterConstants} from '../../shared/typings/router-constants';

@Component({
  selector: 'app-admin-chart-of-accounts-home',
  standalone: false,
  templateUrl: './admin-chart-of-accounts-home.component.html',
  styleUrl: './admin-chart-of-accounts-home.component.scss'
})
export class AdminChartOfAccountsHomeComponent extends BaseComponent implements OnInit {

  menuItems = [
    {
      title: 'Account Categories',
      description: 'Manage top-level account categories (Assets, Liabilities, etc.)',
      icon: 'category',
      route: RouterConstants.ADMIN_ACCOUNT_CATEGORIES,
      color: 'primary'
    },
    {
      title: 'Account Sub-Categories',
      description: 'Manage sub-categories under main categories',
      icon: 'list',
      route: RouterConstants.ADMIN_ACCOUNT_SUBCATEGORIES,
      color: 'accent'
    },
    {
      title: 'Account Types',
      description: 'Manage specific account types under sub-categories',
      icon: 'label',
      route: RouterConstants.ADMIN_ACCOUNT_TYPES,
      color: 'warn'
    },
    {
      title: 'Chart of Accounts',
      description: 'Manage all accounts for your organization',
      icon: 'account_balance_wallet',
      route: RouterConstants.ADMIN_ACCOUNTS,
      color: 'primary'
    }
  ];

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Chart of Accounts');
  }

  protected readonly RouterConstants = RouterConstants;
}
