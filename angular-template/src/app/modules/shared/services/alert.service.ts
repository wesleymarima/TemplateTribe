import {MatSnackBar} from '@angular/material/snack-bar';
import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor(private matSnackBar: MatSnackBar) {
  }

  showError(message: string) {
    this.matSnackBar.open(message, '', {
      duration: 5000, panelClass: 'failure-snackbar'
    });
  }

  showSuccess(message: string) {
    this.matSnackBar.open(message, '', {
      duration: 3000, panelClass: 'success-snackbar'
    });
  }
}
