import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {AuthRestService} from '../../shared/services/rest/auth.rest.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AlertService} from '../../shared/services/alert.service';
import {RouterConstants} from '../../shared/typings/router-constants';

@Component({
  selector: 'app-admin-person-create',
  standalone: false,
  templateUrl: './admin-person-create.component.html',
  styleUrl: './admin-person-create.component.scss'
})
export class AdminPersonCreateComponent extends BaseComponent implements OnInit {
  roles: string[] = [];
  userForm: FormGroup;

  constructor(
    private authRestService: AuthRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.updatePageName('Create User');
    this.userForm = new FormGroup({
      'email': new FormControl(null, [Validators.required, Validators.email]),
      'firstName': new FormControl(null, [Validators.required]),
      'lastName': new FormControl(null, [Validators.required]),
      'role': new FormControl(null, [Validators.required]),
    })
  }

  ngOnInit(): void {
    this.getRoles();
  }

  getRoles() {
    this.authRestService.getRoles().subscribe(response => {
      this.roles = response;
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      this.authRestService.createUser(this.userForm.value).subscribe({
        next: () => {
          this.alertService.showSuccess('User created successfully');
          this.router.navigate([RouterConstants.ADMIN_USERS]);
        },
        error: () => {
          this.alertService.showError('Failed to create user');
        }
      });
    }
  }

}
