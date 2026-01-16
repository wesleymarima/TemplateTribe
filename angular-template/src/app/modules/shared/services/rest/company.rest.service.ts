import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {CompanyDetailDTO, CompanyDTO, CreateCompanyCommand, UpdateCompanyCommand} from '../../models/company';

@Injectable({
  providedIn: 'root'
})
export class CompanyRestService {

  baseURL: string = environment.apiUrl + 'company/';

  constructor(private http: HttpClient) {
  }

  getAll(): Observable<CompanyDTO[]> {
    return this.http.get<CompanyDTO[]>(this.baseURL);
  }

  getById(id: number): Observable<CompanyDetailDTO> {
    return this.http.get<CompanyDetailDTO>(this.baseURL + id);
  }

  create(command: CreateCompanyCommand): Observable<number> {
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateCompanyCommand): Observable<any> {
    return this.http.put(this.baseURL + id, command);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(this.baseURL + id);
  }
}

