import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../../../environments/environment';
import {
  AccountCategoryDTO,
  CreateAccountCategoryCommand,
  UpdateAccountCategoryCommand
} from '../../models/account-category';

@Injectable({
  providedIn: 'root'
})
export class AccountCategoryRestService {
  baseURL: string = environment.apiUrl + 'accountcategory/';

  constructor(private http: HttpClient) {
  }

  getAll(): Observable<AccountCategoryDTO[]> {
    return this.http.get<AccountCategoryDTO[]>(this.baseURL);
  }

  create(command: CreateAccountCategoryCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateAccountCategoryCommand): Observable<any> {
    return this.http.put(this.baseURL + id, command);
  }

  toggleStatus(id: number, isActive: boolean): Observable<any> {
    return this.http.patch(this.baseURL + id + '/status', isActive);
  }
}
