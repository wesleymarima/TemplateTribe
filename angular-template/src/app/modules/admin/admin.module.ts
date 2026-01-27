import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AdminHomeComponent} from './admin-home/admin-home.component';
import {RouterModule} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MaterialModule} from '../shared/material/material.module';
import {SharedModule} from '../shared/shared.module';
import {AdminRoutes} from './admin.routing';
import {AdminPersonsHomeComponent} from './admin-persons-home/admin-persons-home.component';
import {AdminPersonViewComponent} from './admin-person-view/admin-person-view.component';
import {AdminPersonCreateComponent} from './admin-person-create/admin-person-create.component';
import {AdminCompaniesHomeComponent} from './admin-companies-home/admin-companies-home.component';
import {AdminCompanyCreateComponent} from './admin-company-create/admin-company-create.component';
import {AdminCompanyViewComponent} from './admin-company-view/admin-company-view.component';
import {AdminBranchesHomeComponent} from './admin-branches-home/admin-branches-home.component';
import {AdminBranchCreateComponent} from './admin-branch-create/admin-branch-create.component';
import {AdminBranchViewComponent} from './admin-branch-view/admin-branch-view.component';
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
import { AdminChartOfAccountsHomeComponent } from './admin-chart-of-accounts-home/admin-chart-of-accounts-home.component';
import { AdminAccountCategoriesHomeComponent } from './admin-account-categories-home/admin-account-categories-home.component';
import { AdminAccountCategoryCreateComponent } from './admin-account-category-create/admin-account-category-create.component';
import { AdminAccountsHomeComponent } from './admin-accounts-home/admin-accounts-home.component';
import { AdminAccountCreateComponent } from './admin-account-create/admin-account-create.component';
import { AdminAccountViewComponent } from './admin-account-view/admin-account-view.component';
import { AdminAccountTypesHomeComponent } from './admin-account-types-home/admin-account-types-home.component';
import { AdminAccountTypeCreateComponent } from './admin-account-type-create/admin-account-type-create.component';
import { AdminAccountSubcategoriesHomeComponent } from './admin-account-subcategories-home/admin-account-subcategories-home.component';
import { AdminAccountSubcategoryCreateComponent } from './admin-account-subcategory-create/admin-account-subcategory-create.component';
import { AdminJournalEntriesHomeComponent } from './admin-journal-entries-home/admin-journal-entries-home.component';
import { AdminJournalEntryCreateComponent } from './admin-journal-entry-create/admin-journal-entry-create.component';
import { AdminJournalEntryViewComponent } from './admin-journal-entry-view/admin-journal-entry-view.component';


@NgModule({
  declarations: [
    AdminHomeComponent,
    AdminPersonsHomeComponent,
    AdminPersonViewComponent,
    AdminPersonCreateComponent,
    AdminCompaniesHomeComponent,
    AdminCompanyCreateComponent,
    AdminCompanyViewComponent,
    AdminBranchesHomeComponent,
    AdminBranchCreateComponent,
    AdminBranchViewComponent,
    AdminCurrenciesHomeComponent,
    AdminCurrencyCreateComponent,
    AdminCostCentersHomeComponent,
    AdminCostCenterCreateComponent,
    AdminDepartmentsHomeComponent,
    AdminDepartmentCreateComponent,
    AdminExchangeRatesHomeComponent,
    AdminExchangeRateCreateComponent,
    AdminFinancialPeriodsHomeComponent,
    AdminFinancialPeriodCreateComponent,
    AdminChartOfAccountsHomeComponent,
    AdminAccountCategoriesHomeComponent,
    AdminAccountCategoryCreateComponent,
    AdminAccountsHomeComponent,
    AdminAccountCreateComponent,
    AdminAccountViewComponent,
    AdminAccountTypesHomeComponent,
    AdminAccountTypeCreateComponent,
    AdminAccountSubcategoriesHomeComponent,
    AdminAccountSubcategoryCreateComponent,
    AdminJournalEntriesHomeComponent,
    AdminJournalEntryCreateComponent,
    AdminJournalEntryViewComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    SharedModule,
    RouterModule.forChild(AdminRoutes)
  ]
})
export class AdminModule {
}
