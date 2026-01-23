import {SideBarItem} from './side-bar-item';
import {RouterConstants} from './router-constants';

export const AdminSideBarData: SideBarItem[] = [
  {
    icon: 'home',
    path: RouterConstants.ADMIN_HOME,
    name: 'Home'
  },
  {
    icon: 'person',
    path: RouterConstants.ADMIN_USERS,
    name: 'Users'
  },
  {
    icon: 'business',
    path: RouterConstants.ADMIN_COMPANIES,
    name: 'Companies'
  },
  {
    icon: 'store',
    path: RouterConstants.ADMIN_BRANCHES,
    name: 'Branches'
  },
  {
    icon: 'attach_money',
    path: RouterConstants.ADMIN_CURRENCIES,
    name: 'Currencies'
  },
  {
    icon: 'account_balance',
    path: RouterConstants.ADMIN_COST_CENTERS,
    name: 'Cost Centers'
  },
  {
    icon: 'corporate_fare',
    path: RouterConstants.ADMIN_DEPARTMENTS,
    name: 'Departments'
  },
  {
    icon: 'currency_exchange',
    path: RouterConstants.ADMIN_EXCHANGE_RATES,
    name: 'Exchange Rates'
  },
  {
    icon: 'calendar_today',
    path: RouterConstants.ADMIN_FINANCIAL_PERIODS,
    name: 'Financial Periods'
  },
  {
    icon: 'account_tree',
    path: RouterConstants.ADMIN_CHART_OF_ACCOUNTS,
    name: 'Chart of Accounts'
  }
]

export const AuditorSideBarData: SideBarItem[] = [
  {
    icon: 'home',
    path: RouterConstants.AUDIT_HOME,
    name: 'Home'
  }
]
