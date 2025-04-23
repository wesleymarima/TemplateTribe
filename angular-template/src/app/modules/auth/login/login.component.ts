import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../../shared/services/auth.service';
import {AuthRestService} from '../../shared/services/rest/auth.rest.service';
import {Token} from '../../shared/models/token';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  constructor(private authService: AuthService,
              private authRestService: AuthRestService,) {
  }


  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.minLength(6)]),
    password: new FormControl('', [Validators.required]),
  });

  get f() {
    return this.form.controls;
  }

  submit() {
    this.authRestService.login(this.form.value).subscribe(response => {
      const token: Token = response;
      this.authService.saveToken(token);
      this.authService.navigateAccount();
    });

  }
}
