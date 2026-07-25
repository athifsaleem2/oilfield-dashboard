import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { Well, CreateWellRequest, UpdateWellRequest } from '../well.model';

@Injectable({
  providedIn: 'root',
})
export class WellService {
  private readonly baseUrl = `${environment.apiUrl}/Wells`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Well[]> {
    return this.http.get<Well[]>(this.baseUrl);
  }

  getById(id: number): Observable<Well> {
    return this.http.get<Well>(`${this.baseUrl}/${id}`);
  }

  create(well: CreateWellRequest): Observable<number> {
    return this.http.post<number>(this.baseUrl, well);
  }

  update(id: number, well: UpdateWellRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, well);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}