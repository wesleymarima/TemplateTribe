import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {CreateDepartmentCommand, DepartmentDTO, UpdateDepartmentCommand} from '../../models/department';
import {OperationResult} from '../../models/common';

@Injectable({
  providedIn: 'root'
})
export class DepartmentRestService {

  baseURL: string = environment.apiUrl + 'department/';

  constructor(private http: HttpClient) {
  }

  getAll(): Observable<DepartmentDTO[]> {
    return this.http.get<DepartmentDTO[]>(this.baseURL);
  }

  getById(id: number): Observable<DepartmentDTO> {
    return this.http.get<DepartmentDTO>(this.baseURL + id);
  }

  getByCompany(companyId: number): Observable<DepartmentDTO[]> {
    return this.http.get<DepartmentDTO[]>(this.baseURL + 'company/' + companyId);
  }

  create(command: CreateDepartmentCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateDepartmentCommand): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.baseURL + id, command);
  }

  delete(id: number): Observable<OperationResult> {
    return this.http.delete<OperationResult>(this.baseURL + id);
  }
}

