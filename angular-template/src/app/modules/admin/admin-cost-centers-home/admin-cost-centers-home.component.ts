import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {CostCenterRestService} from '../../shared/services/rest/cost-center.rest.service';
import {CostCenterDTO} from '../../shared/models/cost-center';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-cost-centers-home',
  standalone: false,
  templateUrl: './admin-cost-centers-home.component.html',
  styleUrl: './admin-cost-centers-home.component.scss'
})
export class AdminCostCentersHomeComponent extends BaseComponent implements OnInit {

  costCenters: CostCenterDTO[] = [];
  displayedColumns: string[] = ['id', 'code', 'name', 'companyName', 'parentCostCenterName', 'isActive', 'actions'];

  constructor(
    private costCenterRestService: CostCenterRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Cost Centers');
    this.getAll();
  }

  getAll() {
    this.costCenterRestService.getAll().subscribe(response => {
      this.costCenters = response;
    });
  }

  deleteCostCenter(id: number) {
    if (confirm('Are you sure you want to delete this cost center?')) {
      this.costCenterRestService.delete(id).subscribe(() => {
        this.alertService.showSuccess('Cost center deleted successfully');
        this.getAll();
      }, error => {
        this.alertService.showError('Failed to delete cost center: ' + error.error?.message);
      });
    }
  }

  protected readonly RouterConstants = RouterConstants;
}

