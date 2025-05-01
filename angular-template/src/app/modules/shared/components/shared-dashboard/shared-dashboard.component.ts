import {Component, inject} from '@angular/core';
import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {map, Observable, shareReplay} from 'rxjs';
import {AuthService} from '../../services/auth.service';


@Component({
  selector: 'app-shared-dashboard',
  standalone: false,
  templateUrl: './shared-dashboard.component.html',
  styleUrl: './shared-dashboard.component.scss',
})
export class SharedDashboardComponent {

  constructor(private authService: AuthService,) {
  }

  private breakpointObserver = inject(BreakpointObserver);

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  logout() {
    this.authService.logout();
  }

}
