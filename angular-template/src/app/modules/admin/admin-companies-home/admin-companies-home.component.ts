import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {CompanyDTO} from '../../shared/models/company';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-companies-home',
  standalone: false,
  templateUrl: './admin-companies-home.component.html',
  styleUrl: './admin-companies-home.component.scss'
})
export class AdminCompaniesHomeComponent extends BaseComponent implements OnInit {

  companies: CompanyDTO[] = [];
  displayedColumns: string[] = ['id', 'name', 'legalName', 'city', 'country', 'branchesCount', 'isActive', 'actions'];

  constructor(
    private companyRestService: CompanyRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Companies');
    this.getAll();
  }

  getAll() {
    this.companyRestService.getAll().subscribe(response => {
      this.companies = response;
    });
  }

  deleteCompany(id: number) {
    if (confirm('Are you sure you want to delete this company?')) {
      this.companyRestService.delete(id).subscribe(() => {
        this.alertService.showSuccess('Company deleted successfully');
        this.getAll();
      });
    }
  }

  protected readonly RouterConstants = RouterConstants;
}
