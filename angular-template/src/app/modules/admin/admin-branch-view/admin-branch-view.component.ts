import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {BranchRestService} from '../../shared/services/rest/branch.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {BranchDetailDTO, UpdateBranchCommand} from '../../shared/models/branch';
import {CompanyDTO} from '../../shared/models/company';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {BaseComponent} from '../../shared/typings/base-component';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-branch-view',
  standalone: false,
  templateUrl: './admin-branch-view.component.html',
  styleUrl: './admin-branch-view.component.scss'
})
export class AdminBranchViewComponent extends BaseComponent implements OnInit {
  id: number;
  branch?: BranchDetailDTO;
  companies: CompanyDTO[] = [];
  branchForm: FormGroup;
  isEditing = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private branchRestService: BranchRestService,
    private companyRestService: CompanyRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.id = this.activatedRoute.snapshot.params['id'];
    this.branchForm = new FormGroup({
      'name': new FormControl(null, [Validators.required]),
      'code': new FormControl(null, [Validators.required]),
      'email': new FormControl(null, [Validators.email]),
      'phone': new FormControl(null),
      'description': new FormControl(null),
      'addressLine1': new FormControl(null),
      'addressLine2': new FormControl(null),
      'city': new FormControl(null),
      'state': new FormControl(null),
      'postalCode': new FormControl(null),
      'country': new FormControl(null),
      'branchType': new FormControl(null, [Validators.required]),
      'isHeadquarters': new FormControl(false),
      'isActive': new FormControl(true),
      'businessHours': new FormControl(null),
      'managerId': new FormControl(null)
    });
  }

  ngOnInit(): void {
    this.getBranchById();
    this.getCompanies();
  }

  getBranchById() {
    this.branchRestService.getById(this.id).subscribe(response => {
      this.branch = response;
      this.updatePageName('Branch: ' + response.name);
      this.branchForm.patchValue(response);
    });
  }

  getCompanies() {
    this.companyRestService.getAll().subscribe(response => {
      this.companies = response;
    });
  }

  toggleEdit() {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.branchForm.patchValue(this.branch!);
    }
  }

  onSubmit() {
    if (this.branchForm.valid) {
      const updateCommand: UpdateBranchCommand = {
        id: this.id,
        ...this.branchForm.value
      };
      this.branchRestService.update(this.id, updateCommand).subscribe({
        next: () => {
          this.alertService.showSuccess('Branch updated successfully');
          this.isEditing = false;
          this.getBranchById();
        },
        error: () => {
          this.alertService.showError('Failed to update branch');
        }
      });
    }
  }

  deleteBranch() {
    if (confirm('Are you sure you want to delete this branch?')) {
      this.branchRestService.delete(this.id).subscribe({
        next: () => {
          this.alertService.showSuccess('Branch deleted successfully');
          this.router.navigate([RouterConstants.ADMIN_BRANCHES]);
        },
        error: () => {
          this.alertService.showError('Failed to delete branch');
        }
      });
    }
  }

  protected readonly RouterConstants = RouterConstants;
}
