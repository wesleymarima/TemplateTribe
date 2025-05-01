import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {PaginationService} from '../pagination.service';
import {Observable, tap} from 'rxjs';
import {PaginationMetadata} from '../../typings/pagination-meta-data';


@Injectable({providedIn: 'root'})
export class PaginatedHttpService {
  constructor(private http: HttpClient, private pagination: PaginationService) {
  }

  private handlePagination<T>(obs: Observable<any>): Observable<any> {
    console.log('handleee')
    return obs.pipe(
      tap((response: any) => {
        console.log(response);
        var mm: PaginationMetadata = {
          pageNumber: response.pageNumber,
          pageSize: response.pageSize,
          totalCount: response.totalCount,
          hasNextPage: response.hasNextPage,
          hasPreviousPage: response.hasPreviousPage,
          totalItems: response.totalItems,
          totalPages: response.totalPages,
        }
        this.pagination.update(mm);
        // if (response?.pagination) {
        //   console.log(response)
        //   this.pagination.update(response.pagination);
        // }
      })
    );
  }

  // get<any>(url: string, options?: any): Observable<any> {
  //   return this.handlePagination(this.http.get<any>(url, options));
  // }

  post<T>(url: string, body: any, options?: any): Observable<T> {
    return this.handlePagination(this.http.post<T>(url, body, options));
  }

  put<T>(url: string, body: any, options?: any): Observable<T> {
    return this.handlePagination(this.http.put<T>(url, body, options));
  }

  delete<T>(url: string, options?: any): Observable<T> {
    return this.handlePagination(this.http.delete<T>(url, options));
  }
}
