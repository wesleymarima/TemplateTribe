import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {PersonRestService} from '../../shared/services/rest/person.rest.service';
import {Person} from '../../shared/models/person';
import {RouterConstants} from '../../shared/typings/router-constants';

@Component({
  selector: 'app-admin-persons-home',
  standalone: false,
  templateUrl: './admin-persons-home.component.html',
  styleUrl: './admin-persons-home.component.scss'
})
export class AdminPersonsHomeComponent extends BaseComponent implements OnInit {

  person: Person[] = [];
  displayedColumns: string[] = ['id', 'name', 'email', 'role', 'view'];

  constructor(private personRestService: PersonRestService) {
    super()
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Users');
    this.getAll();
  }

  getAll() {
    this.personRestService.getAll(this.pageNumber, this.pageSize).subscribe(response => {
      this.person = response.items;
      this.updateData(response);
    });
  }

  protected readonly RouterConstants = RouterConstants;
}
