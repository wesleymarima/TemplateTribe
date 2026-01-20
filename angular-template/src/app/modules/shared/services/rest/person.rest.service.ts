import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {BaseResponse} from '../../models/base-response';
import {Person} from '../../models/person';
import {Observable} from 'rxjs';
import {ChangePersonRoleCommand} from '../../models/common';

@Injectable({
  providedIn: 'root'
})
export class PersonRestService {

  baseURL: string = environment.apiUrl + 'persons/';

  constructor(private http: HttpClient) {
  }

  getAll(pageNumber: number, pageSize: number): Observable<BaseResponse<Person>> {
    return this.http.get<BaseResponse<Person>>(this.baseURL + 'all', {
      params: {
        'pageNumber': pageNumber,
        'pageSize': pageSize,
      }
    });
  }

  getById(id: number): Observable<Person> {
    return this.http.get<Person>(this.baseURL + 'getbyid/' + id);
  }

  updateRole(command: ChangePersonRoleCommand): Observable<any> {
    return this.http.post(this.baseURL + 'updaterole', command);
  }

}
