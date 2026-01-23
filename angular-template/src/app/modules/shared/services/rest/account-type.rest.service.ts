import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../../../environments/environment';
import {AccountTypeDTO, CreateAccountTypeCommand, UpdateAccountTypeCommand} from '../../models/account-type';

@Injectable({
  providedIn: 'root'
})
export class AccountTypeRestService {
  baseURL: string = environment.apiUrl + 'accounttype/';

  constructor(private http: HttpClient) {
  }

  getAll(accountSubCategoryId?: number): Observable<AccountTypeDTO[]> {
    if (accountSubCategoryId) {
      return this.http.get<AccountTypeDTO[]>(this.baseURL, {
        params: {accountSubCategoryId: accountSubCategoryId.toString()}
      });
    }
    return this.http.get<AccountTypeDTO[]>(this.baseURL);
  }

  create(command: CreateAccountTypeCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateAccountTypeCommand): Observable<any> {
    return this.http.put(this.baseURL + id, command);
  }

  toggleStatus(id: number, isActive: boolean): Observable<any> {
    return this.http.patch(this.baseURL + id + '/status', isActive);
  }
}
