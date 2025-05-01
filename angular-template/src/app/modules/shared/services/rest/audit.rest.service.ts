import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {BaseResponse} from '../../models/base-response';
import {Audit} from '../../models/audit';
import {PaginatedHttpService} from './paginated-http.service';

@Injectable({
  providedIn: 'root'
})
export class AuditRestService {

  baseURL: string = environment.apiUrl + 'audits/';

  constructor(private http: HttpClient,
              private paginatedHttpService: PaginatedHttpService) {
  }

  getTables(): Observable<string[]> {
    return this.http.get<string[]>(this.baseURL + 'tablenames');
  }

  getAll(pageNumber: number, pageSize: number): Observable<BaseResponse<Audit>> {
    return this.http.get<BaseResponse<Audit>>(this.baseURL + 'getall', {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize
      }
    });
  }

  filterData(model: any): Observable<BaseResponse<Audit>> {
    // return this.http.post<BaseResponse<Audit>>(this.baseURL + 'range', model);
    return this.paginatedHttpService.post<BaseResponse<Audit>>(this.baseURL + 'range', model);
  }
}
