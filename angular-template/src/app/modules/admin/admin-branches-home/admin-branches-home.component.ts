import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {BranchRestService} from '../../shared/services/rest/branch.rest.service';
import {BranchDTO} from '../../shared/models/branch';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-branches-home',
  standalone: false,
  templateUrl: './admin-branches-home.component.html',
  styleUrl: './admin-branches-home.component.scss'
})
export class AdminBranchesHomeComponent extends BaseComponent implements OnInit {

  branches: BranchDTO[] = [];
  displayedColumns: string[] = ['id', 'name', 'code', 'city', 'companyName', 'branchType', 'isHeadquarters', 'isActive', 'actions'];

  constructor(
    private branchRestService: BranchRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Branches');
    this.getAll();
  }

  getAll() {
    this.branchRestService.getAll().subscribe(response => {
      this.branches = response;
    });
  }

  deleteBranch(id: number) {
    if (confirm('Are you sure you want to delete this branch?')) {
      this.branchRestService.delete(id).subscribe(() => {
        this.alertService.showSuccess('Branch deleted successfully');
        this.getAll();
      });
    }
  }

  protected readonly RouterConstants = RouterConstants;
}
