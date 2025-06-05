import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {PersonRestService} from '../../shared/services/rest/person.rest.service';
import {Person} from '../../shared/models/person';
import {AuthRestService} from '../../shared/services/rest/auth.rest.service';

@Component({
  selector: 'app-admin-person-view',
  standalone: false,
  templateUrl: './admin-person-view.component.html',
  styleUrl: './admin-person-view.component.scss'
})
export class AdminPersonViewComponent implements OnInit {
  id: number;
  person?: Person;
  roles: string[] = [];

  constructor(private activatedRoute: ActivatedRoute,
              private personRestService: PersonRestService,
              private authRestService: AuthRestService) {
    this.id = this.activatedRoute.snapshot.params['id'];
  }

  ngOnInit(): void {
    this.getPersonById();
    this.getRoles();
  }

  getPersonById() {
    this.personRestService.getById(this.id).subscribe(response => {
      this.person = response;
    });
  }

  getRoles() {
    this.authRestService.getRoles().subscribe(response => {
      this.roles = response;
    });
  }

}
