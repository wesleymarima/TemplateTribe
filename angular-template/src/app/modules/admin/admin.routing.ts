import {Routes} from '@angular/router';
import {SharedDashboardComponent} from '../shared/components/shared-dashboard/shared-dashboard.component';
import {AdminHomeComponent} from './admin-home/admin-home.component';
import {AdminPersonsHomeComponent} from './admin-persons-home/admin-persons-home.component';
import {AdminPersonViewComponent} from './admin-person-view/admin-person-view.component';
import {AdminPersonCreateComponent} from './admin-person-create/admin-person-create.component';
import {AdminCompaniesHomeComponent} from './admin-companies-home/admin-companies-home.component';
import {AdminCompanyViewComponent} from './admin-company-view/admin-company-view.component';
import {AdminCompanyCreateComponent} from './admin-company-create/admin-company-create.component';
import {AdminBranchesHomeComponent} from './admin-branches-home/admin-branches-home.component';
import {AdminBranchViewComponent} from './admin-branch-view/admin-branch-view.component';
import {AdminBranchCreateComponent} from './admin-branch-create/admin-branch-create.component';
import {AdminCurrenciesHomeComponent} from './admin-currencies-home/admin-currencies-home.component';
import {AdminCurrencyCreateComponent} from './admin-currency-create/admin-currency-create.component';
import {AdminCostCentersHomeComponent} from './admin-cost-centers-home/admin-cost-centers-home.component';
import {AdminCostCenterCreateComponent} from './admin-cost-center-create/admin-cost-center-create.component';
import {AdminDepartmentsHomeComponent} from './admin-departments-home/admin-departments-home.component';
import {AdminDepartmentCreateComponent} from './admin-department-create/admin-department-create.component';
import {AdminExchangeRatesHomeComponent} from './admin-exchange-rates-home/admin-exchange-rates-home.component';
import {AdminExchangeRateCreateComponent} from './admin-exchange-rate-create/admin-exchange-rate-create.component';
import {
  AdminFinancialPeriodsHomeComponent
} from './admin-financial-periods-home/admin-financial-periods-home.component';
import {
  AdminFinancialPeriodCreateComponent
} from './admin-financial-period-create/admin-financial-period-create.component';

export const AdminRoutes: Routes = [
  {
    path: '', component: SharedDashboardComponent, children: [
      {path: 'home', component: AdminHomeComponent},
      {path: 'persons', component: AdminPersonsHomeComponent},
      {path: 'person-create', component: AdminPersonCreateComponent},
      {path: 'person-view/:id', component: AdminPersonViewComponent},
      {path: 'companies', component: AdminCompaniesHomeComponent},
      {path: 'company-create', component: AdminCompanyCreateComponent},
      {path: 'company-view/:id', component: AdminCompanyViewComponent},
      {path: 'branches', component: AdminBranchesHomeComponent},
      {path: 'branch-create', component: AdminBranchCreateComponent},
      {path: 'branch-view/:id', component: AdminBranchViewComponent},
      {path: 'currencies', component: AdminCurrenciesHomeComponent},
      {path: 'currency-create', component: AdminCurrencyCreateComponent},
      {path: 'cost-centers', component: AdminCostCentersHomeComponent},
      {path: 'cost-center-create', component: AdminCostCenterCreateComponent},
      {path: 'departments', component: AdminDepartmentsHomeComponent},
      {path: 'department-create', component: AdminDepartmentCreateComponent},
      {path: 'exchange-rates', component: AdminExchangeRatesHomeComponent},
      {path: 'exchange-rate-create', component: AdminExchangeRateCreateComponent},
      {path: 'financial-periods', component: AdminFinancialPeriodsHomeComponent},
      {path: 'financial-period-create', component: AdminFinancialPeriodCreateComponent}
    ]
  }
]
