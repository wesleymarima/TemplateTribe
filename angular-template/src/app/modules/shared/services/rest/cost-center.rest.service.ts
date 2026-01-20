import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {CostCenterDTO, CreateCostCenterCommand, UpdateCostCenterCommand} from '../../models/cost-center';
import {OperationResult} from '../../models/common';

@Injectable({
  providedIn: 'root'
})
export class CostCenterRestService {

  baseURL: string = environment.apiUrl + 'costcenter/';

  constructor(private http: HttpClient) {
  }

  getAll(): Observable<CostCenterDTO[]> {
    return this.http.get<CostCenterDTO[]>(this.baseURL);
  }

  getById(id: number): Observable<CostCenterDTO> {
    return this.http.get<CostCenterDTO>(this.baseURL + id);
  }

  getByCompany(companyId: number): Observable<CostCenterDTO[]> {
    return this.http.get<CostCenterDTO[]>(this.baseURL + 'company/' + companyId);
  }

  create(command: CreateCostCenterCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateCostCenterCommand): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.baseURL + id, command);
  }

  delete(id: number): Observable<OperationResult> {
    return this.http.delete<OperationResult>(this.baseURL + id);
  }
}

