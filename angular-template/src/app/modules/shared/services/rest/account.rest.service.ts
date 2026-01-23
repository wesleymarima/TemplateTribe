import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../../../environments/environment';
import {
  AccountDetailDTO,
  AccountDTO,
  AccountLedgerResponse,
  CreateAccountCommand,
  SetOpeningBalanceCommand,
  UpdateAccountCommand
} from '../../models/account';

@Injectable({
  providedIn: 'root'
})
export class AccountRestService {
  baseURL: string = environment.apiUrl + 'account/';

  constructor(private http: HttpClient) {
  }

  getAll(companyId?: number): Observable<AccountDTO[]> {
    if (companyId) {
      return this.http.get<AccountDTO[]>(this.baseURL, {
        params: {companyId: companyId.toString()}
      });
    }
    return this.http.get<AccountDTO[]>(this.baseURL);
  }

  getById(id: number): Observable<AccountDetailDTO> {
    return this.http.get<AccountDetailDTO>(this.baseURL + id);
  }

  create(command: CreateAccountCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateAccountCommand): Observable<any> {
    return this.http.put(this.baseURL + id, command);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(this.baseURL + id);
  }

  toggleStatus(id: number, isActive: boolean): Observable<any> {
    return this.http.patch(this.baseURL + id + '/status', isActive);
  }

  setOpeningBalance(id: number, command: SetOpeningBalanceCommand): Observable<any> {
    return this.http.post(this.baseURL + id + '/opening-balance', command);
  }

  getLedger(
    id: number,
    startDate?: Date,
    endDate?: Date,
    pageNumber: number = 1,
    pageSize: number = 50
  ): Observable<AccountLedgerResponse> {
    let params: any = {pageNumber: pageNumber.toString(), pageSize: pageSize.toString()};
    if (startDate) params.startDate = startDate.toISOString();
    if (endDate) params.endDate = endDate.toISOString();
    return this.http.get<AccountLedgerResponse>(this.baseURL + id + '/ledger', {params});
  }
}
