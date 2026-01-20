import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {
  CreateFinancialPeriodCommand,
  FinancialPeriodDTO,
  ReopenFinancialPeriodCommand
} from '../../models/financial-period';
import {OperationResult} from '../../models/common';

@Injectable({
  providedIn: 'root'
})
export class FinancialPeriodRestService {

  baseURL: string = environment.apiUrl + 'financialperiod/';

  constructor(private http: HttpClient) {
  }

  getByCompany(companyId: number): Observable<FinancialPeriodDTO[]> {
    return this.http.get<FinancialPeriodDTO[]>(this.baseURL + 'company/' + companyId);
  }

  getOpenPeriods(companyId: number): Observable<FinancialPeriodDTO[]> {
    return this.http.get<FinancialPeriodDTO[]>(this.baseURL + 'company/' + companyId + '/open');
  }

  create(command: CreateFinancialPeriodCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  close(id: number): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.baseURL + id + '/close', {});
  }

  reopen(id: number, command: ReopenFinancialPeriodCommand): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.baseURL + id + '/reopen', command);
  }

  delete(id: number): Observable<OperationResult> {
    return this.http.delete<OperationResult>(this.baseURL + id);
  }
}

