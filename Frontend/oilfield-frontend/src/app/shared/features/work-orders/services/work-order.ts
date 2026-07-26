import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { WorkOrder, CreateWorkOrderRequest, UpdateWorkOrderRequest } from '../work-order.model';

@Injectable({
  providedIn: 'root',
})
export class WorkOrderService {
  private readonly baseUrl = `${environment.apiUrl}/WorkOrders`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<WorkOrder[]> {
    return this.http.get<WorkOrder[]>(this.baseUrl);
  }

  create(order: CreateWorkOrderRequest): Observable<number> {
    return this.http.post<number>(this.baseUrl, order);
  }

  update(id: number, order: UpdateWorkOrderRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, order);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}