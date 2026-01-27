import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {BaseComponent} from '../../shared/typings/base-component';
import {JournalEntryRestService} from '../../shared/services/rest/journal-entry.rest.service';
import {JournalEntryDetailDTO} from '../../shared/models/journal-entry';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-journal-entry-view',
  standalone: false,
  templateUrl: './admin-journal-entry-view.component.html',
  styleUrl: './admin-journal-entry-view.component.scss'
})
export class AdminJournalEntryViewComponent extends BaseComponent implements OnInit {

  journalEntryId!: number;
  journalEntry?: JournalEntryDetailDTO;
  displayedColumns: string[] = ['lineNumber', 'accountCode', 'accountName', 'description', 'debitAmount', 'creditAmount', 'costCenter', 'department'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private journalEntryRestService: JournalEntryRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.journalEntryId = +this.route.snapshot.params['id'];
    this.loadJournalEntry();
  }

  loadJournalEntry() {
    this.journalEntryRestService.getById(this.journalEntryId).subscribe(entry => {
      this.journalEntry = entry;
      this.baseSharedService.updateTitle(`Journal Entry: ${entry.journalNumber}`);
    });
  }

  postJournalEntry() {
    if (confirm('Are you sure you want to post this journal entry? This action will update account balances and cannot be undone.')) {
      this.journalEntryRestService.post(this.journalEntryId).subscribe({
        next: () => {
          this.alertService.showSuccess('Journal entry posted successfully');
          this.loadJournalEntry();
        },
        error: () => {
          this.alertService.showError('Failed to post journal entry');
        }
      });
    }
  }

  reverseJournalEntry() {
    const description = prompt('Enter reversal reason:');
    if (description) {
      this.journalEntryRestService.reverse(this.journalEntryId, {
        reversalDate: new Date().toISOString(),
        description
      }).subscribe({
        next: (newId) => {
          this.alertService.showSuccess('Journal entry reversed successfully');
          this.router.navigate([RouterConstants.ADMIN_JOURNAL_ENTRY_VIEW, newId]);
        },
        error: () => {
          this.alertService.showError('Failed to reverse journal entry');
        }
      });
    }
  }

  deleteJournalEntry() {
    if (confirm('Are you sure you want to delete this journal entry?')) {
      this.journalEntryRestService.delete(this.journalEntryId).subscribe({
        next: () => {
          this.alertService.showSuccess('Journal entry deleted successfully');
          this.router.navigate([RouterConstants.ADMIN_JOURNAL_ENTRIES]);
        },
        error: () => {
          this.alertService.showError('Failed to delete journal entry');
        }
      });
    }
  }

  goBack() {
    this.router.navigate([RouterConstants.ADMIN_JOURNAL_ENTRIES]);
  }

  protected readonly RouterConstants = RouterConstants;
}
