import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { Alert } from '../alert.model';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private readonly baseUrl = `${environment.apiUrl}/Alerts`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Alert[]> {
    return this.http.get<Alert[]>(this.baseUrl);
  }

  resolve(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/resolve`, {});
  }
}