import {Component} from '@angular/core';
import {AuthService} from '../shared/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'angular-template';

  constructor(private authService: AuthService) {

    if (this.authService.isTokenValid()) {
      this.authService.navigateAccount();
    } else {
      this.authService.logout();
    }

    // if (this.authService.isLoggedIn()) {
    //   this.authService.navigateAccount();
    // } else {
    //   this.authService.logout();
    // }
  }
}
