import {Injectable} from '@angular/core';
import {JwtHelperService} from '@auth0/angular-jwt';
import {Router} from '@angular/router';
import {AlertService} from './alert.service';
import {Token} from '../models/token';
import {AccountType} from '../typings/account-type';
import {RouterConstants} from '../typings/router-constants';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  jwtHelper = new JwtHelperService();

  constructor(private router: Router,
              private alertService: AlertService) {
  }

  getToken(): string {
    const token = localStorage.getItem('token');
    if (token == null) {
      return '';
    }
    return token.toString();
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    if (token === null) {
      return false;
    }
    return true;
  }

  getRole(): string {
    const role = localStorage.getItem('role');
    if (role == null) {
      return '';
    }
    return role;
  }

  getName(): string {
    const name = localStorage.getItem('name');
    return name ? name.toUpperCase() : '';
  }

  isTokenValid(): boolean {
    const token = localStorage.getItem('token');

    if (token === null || '') {
      return false;
    }
    const decode: any = this.jwtHelper.decodeToken(this.getToken());
    return decode.exp * 1000 > Date.now();
  }

  getPersonId(): string {
    let personId = localStorage.getItem('personId');
    return personId ? personId : '';
  }

  logout() {
    localStorage.clear();
    console.log('log out service');
    this.router.navigate(['/auth/login']);
  }

  saveToken(token: Token) {
    localStorage.setItem('token', token.token);
    localStorage.setItem('id', token.id);
    localStorage.setItem('userName', token.userName);
    localStorage.setItem('email', token.email);
    localStorage.setItem('role', token.role);
    localStorage.setItem('personId', token.personId);
    localStorage.setItem('name', token.name);
  }

  navigateAccount() {
    if (this.isLoggedIn()) {
      let accountType = localStorage.getItem('role');
      console.log(accountType);
      switch (accountType) {
        case AccountType.ADMINISTRATOR:
          this.router.navigate([RouterConstants.ADMIN_HOME]);
          // this.router.navigate(['/admin/home']);
          break;
        case AccountType.AUDITOR:
          this.router.navigate([RouterConstants.AUDIT_HOME]);
          break;
        default:
          this.alertService.showError('Account type not found');
          this.logout();
      }
    } else {
      this.alertService.showError('Token expired, please login again');
      this.logout();
    }
  }
}
