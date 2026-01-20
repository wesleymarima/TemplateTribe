import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {CreateCurrencyCommand, CurrencyDTO, UpdateCurrencyCommand} from '../../models/currency';
import {OperationResult} from '../../models/common';

@Injectable({
  providedIn: 'root'
})
export class CurrencyRestService {

  baseURL: string = environment.apiUrl + 'currency/';

  constructor(private http: HttpClient) {
  }

  getAll(): Observable<CurrencyDTO[]> {
    return this.http.get<CurrencyDTO[]>(this.baseURL);
  }

  getActive(): Observable<CurrencyDTO[]> {
    return this.http.get<CurrencyDTO[]>(this.baseURL + 'active');
  }

  getById(id: number): Observable<CurrencyDTO> {
    return this.http.get<CurrencyDTO>(this.baseURL + id);
  }

  create(command: CreateCurrencyCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateCurrencyCommand): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.baseURL + id, command);
  }

  delete(id: number): Observable<OperationResult> {
    return this.http.delete<OperationResult>(this.baseURL + id);
  }
}

