import {Component, inject} from '@angular/core';
import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {map, Observable, shareReplay} from 'rxjs';
import {AuthService} from '../../services/auth.service';
import {SideBarItem} from '../../typings/side-bar-item';
import {AccountType} from '../../typings/account-type';
import {AdminSideBarData, AuditorSideBarData} from '../../typings/sidebar-data';
import {BaseSharedService} from '../../services/base.shared.service';


@Component({
  selector: 'app-shared-dashboard',
  standalone: false,
  templateUrl: './shared-dashboard.component.html',
  styleUrl: './shared-dashboard.component.scss',
})
export class SharedDashboardComponent {
  sidebarItems: SideBarItem[] = [];
  baseSharedService = inject(BaseSharedService);

  constructor(private authService: AuthService,) {
    let role = authService.getRole();
    switch (role) {
      case AccountType.ADMIN:
        this.sidebarItems = AdminSideBarData;
        break;
      case AccountType.AUDITOR:
        this.sidebarItems = AuditorSideBarData;
        break;
    }
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

  getTitle() {
    return this.baseSharedService.dashboardTitle();
  }

}
