import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../../../environments/environment';
import {
  CreateJournalEntryCommand,
  JournalEntryDetailDTO,
  JournalEntryDTO,
  ReverseJournalEntryRequest,
  UpdateJournalEntryCommand
} from '../../models/journal-entry';

@Injectable({
  providedIn: 'root'
})
export class JournalEntryRestService {
  baseURL: string = environment.apiUrl + 'JournalEntry/';

  constructor(private http: HttpClient) {
  }

  getAll(financialPeriodId?: number): Observable<JournalEntryDTO[]> {
    if (financialPeriodId) {
      return this.http.get<JournalEntryDTO[]>(this.baseURL, {
        params: {financialPeriodId: financialPeriodId.toString()}
      });
    }
    return this.http.get<JournalEntryDTO[]>(this.baseURL);
  }

  getById(id: number): Observable<JournalEntryDetailDTO> {
    return this.http.get<JournalEntryDetailDTO>(this.baseURL + id);
  }

  create(command: CreateJournalEntryCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateJournalEntryCommand): Observable<any> {
    return this.http.put(this.baseURL + id, command);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(this.baseURL + id);
  }

  post(id: number): Observable<any> {
    return this.http.post(this.baseURL + id + '/post', {});
  }

  reverse(id: number, request: ReverseJournalEntryRequest): Observable<number> {
    return this.http.post<number>(this.baseURL + id + '/reverse', request);
  }
}
