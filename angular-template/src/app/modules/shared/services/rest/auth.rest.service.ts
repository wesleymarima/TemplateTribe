import {Observable} from "rxjs";
import {environment} from '../../../../../environments/environment';
import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Token} from '../../models/token';

@Injectable({
  providedIn: 'root'
})
export class AuthRestService {
  baseURL: string = environment.apiUrl + 'auth/';

  constructor(private http: HttpClient) {
  }

  login(model: any): Observable<Token> {
    return this.http.post<Token>(this.baseURL + 'login', model)
  }

  getRoles(): Observable<string[]> {
    return this.http.get<string[]>(this.baseURL + 'roles');
  }

  createUser(model: any): Observable<string> {
    return this.http.post<string>(this.baseURL + 'create', model);
  }
}
