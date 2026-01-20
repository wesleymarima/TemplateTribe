import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {CreateExchangeRateCommand, ExchangeRateDTO, UpdateExchangeRateCommand} from '../../models/exchange-rate';
import {OperationResult} from '../../models/common';

@Injectable({
  providedIn: 'root'
})
export class ExchangeRateRestService {

  baseURL: string = environment.apiUrl + 'exchangerate/';

  constructor(private http: HttpClient) {
  }

  getByCurrency(currencyId: number, companyId: number): Observable<ExchangeRateDTO[]> {
    return this.http.get<ExchangeRateDTO[]>(
      this.baseURL + 'currency/' + currencyId + '/company/' + companyId
    );
  }

  getLatest(currencyId: number, toCurrencyCode: string, companyId: number, asOfDate?: string): Observable<ExchangeRateDTO> {
    let params = new HttpParams()
      .set('currencyId', currencyId.toString())
      .set('toCurrencyCode', toCurrencyCode)
      .set('companyId', companyId.toString());

    if (asOfDate) {
      params = params.set('asOfDate', asOfDate);
    }

    return this.http.get<ExchangeRateDTO>(this.baseURL + 'latest', {params});
  }

  create(command: CreateExchangeRateCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateExchangeRateCommand): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.baseURL + id, command);
  }

  delete(id: number): Observable<OperationResult> {
    return this.http.delete<OperationResult>(this.baseURL + id);
  }
}

