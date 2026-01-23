import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../../../environments/environment';
import {
  AccountSubCategoryDTO,
  CreateAccountSubCategoryCommand,
  UpdateAccountSubCategoryCommand
} from '../../models/account-subcategory';

@Injectable({
  providedIn: 'root'
})
export class AccountSubCategoryRestService {
  baseURL: string = environment.apiUrl + 'accountsubcategory/';

  constructor(private http: HttpClient) {
  }

  getAll(accountCategoryId?: number): Observable<AccountSubCategoryDTO[]> {
    if (accountCategoryId) {
      return this.http.get<AccountSubCategoryDTO[]>(this.baseURL, {
        params: {accountCategoryId: accountCategoryId.toString()}
      });
    }
    return this.http.get<AccountSubCategoryDTO[]>(this.baseURL);
  }

  create(command: CreateAccountSubCategoryCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateAccountSubCategoryCommand): Observable<any> {
    return this.http.put(this.baseURL + id, command);
  }

  toggleStatus(id: number, isActive: boolean): Observable<any> {
    return this.http.patch(this.baseURL + id + '/status', isActive);
  }
}
