import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {BaseComponent} from '../../shared/typings/base-component';
import {AccountRestService} from '../../shared/services/rest/account.rest.service';
import {AccountDetailDTO, AccountLedgerResponse} from '../../shared/models/account';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-admin-account-view',
  standalone: false,
  templateUrl: './admin-account-view.component.html',
  styleUrl: './admin-account-view.component.scss'
})
export class AdminAccountViewComponent extends BaseComponent implements OnInit {

  accountId!: number;
  account?: AccountDetailDTO;
  ledger?: AccountLedgerResponse;
  editForm: FormGroup;
  isEditMode = false;
  displayedColumns: string[] = ['transactionDate', 'description', 'referenceNumber', 'debitAmount', 'creditAmount', 'runningBalance'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private accountRestService: AccountRestService,
    private alertService: AlertService,
    private fb: FormBuilder
  ) {
    super();
    this.editForm = this.fb.group({
      accountName: ['', Validators.required],
      description: [''],
      isActive: [true],
      allowDirectPosting: [true],
      requiresCostCenter: [false],
      requiresDepartment: [false],
      requiresBranch: [false]
    });
  }

  ngOnInit(): void {
    this.accountId = +this.route.snapshot.params['id'];
    this.loadAccount();
    this.loadLedger();
  }

  loadAccount() {
    this.accountRestService.getById(this.accountId).subscribe(account => {
      this.account = account;
      this.baseSharedService.updateTitle(`Account: ${account.accountCode}`);
      this.editForm.patchValue({
        accountName: account.accountName,
        description: account.description,
        isActive: account.isActive,
        allowDirectPosting: account.allowDirectPosting,
        requiresCostCenter: account.requiresCostCenter,
        requiresDepartment: account.requiresDepartment,
        requiresBranch: account.requiresBranch
      });
    });
  }

  loadLedger() {
    this.accountRestService.getLedger(this.accountId).subscribe(ledger => {
      this.ledger = ledger;
    });
  }

  toggleEditMode() {
    this.isEditMode = !this.isEditMode;
    if (!this.isEditMode && this.account) {
      this.editForm.patchValue({
        accountName: this.account.accountName,
        description: this.account.description,
        isActive: this.account.isActive,
        allowDirectPosting: this.account.allowDirectPosting,
        requiresCostCenter: this.account.requiresCostCenter,
        requiresDepartment: this.account.requiresDepartment,
        requiresBranch: this.account.requiresBranch
      });
    }
  }

  saveChanges() {
    if (this.editForm.valid) {
      const command = {
        id: this.accountId,
        ...this.editForm.value
      };
      this.accountRestService.update(this.accountId, command).subscribe({
        next: () => {
          this.alertService.showSuccess('Account updated successfully');
          this.isEditMode = false;
          this.loadAccount();
        },
        error: () => {
          this.alertService.showError('Failed to update account');
        }
      });
    }
  }

  goBack() {
    this.router.navigate([RouterConstants.ADMIN_ACCOUNTS]);
  }

  protected readonly RouterConstants = RouterConstants;
}
