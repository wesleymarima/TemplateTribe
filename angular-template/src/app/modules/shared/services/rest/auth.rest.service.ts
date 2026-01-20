import {Observable} from "rxjs";
import {environment} from '../../../../../environments/environment';
import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Token} from '../../models/token';
import {AuthenticationRequest, NewUser} from '../../models/common';

@Injectable({
  providedIn: 'root'
})
export class AuthRestService {
  baseURL: string = environment.apiUrl + 'auth/';

  constructor(private http: HttpClient) {
  }

  login(loginRequest: AuthenticationRequest): Observable<Token> {
    return this.http.post<Token>(this.baseURL + 'login', loginRequest);
  }

  test(): Observable<any> {
    return this.http.get<any>(this.baseURL + 'test');
  }

  getRoles(): Observable<string[]> {
    return this.http.get<string[]>(this.baseURL + 'roles');
  }

  createUser(createUser: NewUser): Observable<any> {
    return this.http.post<any>(this.baseURL + 'create', createUser);
  }
}
