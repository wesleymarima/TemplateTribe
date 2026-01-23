import {Injectable} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {BranchDetailDTO, BranchDTO, CreateBranchCommand, UpdateBranchCommand} from '../../models/branch';

@Injectable({
  providedIn: 'root'
})
export class BranchRestService {

  baseURL: string = environment.apiUrl + 'branch/';

  constructor(private http: HttpClient) {
  }

  getAll(): Observable<BranchDTO[]> {
    return this.http.get<BranchDTO[]>(this.baseURL);
  }

  getById(id: number): Observable<BranchDetailDTO> {
    return this.http.get<BranchDetailDTO>(this.baseURL + id);
  }

  getCurrent(): Observable<BranchDetailDTO> {
    return this.http.get<BranchDetailDTO>(this.baseURL + 'current');
  }

  getByCompany(companyId: number): Observable<BranchDTO[]> {
    return this.http.get<BranchDTO[]>(this.baseURL + 'company/' + companyId);
  }

  create(command: CreateBranchCommand): Observable<number> {
    console.log(command);
    
    return this.http.post<number>(this.baseURL, command);
  }

  update(id: number, command: UpdateBranchCommand): Observable<any> {
    return this.http.put(this.baseURL + id, command);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(this.baseURL + id);
  }
}

