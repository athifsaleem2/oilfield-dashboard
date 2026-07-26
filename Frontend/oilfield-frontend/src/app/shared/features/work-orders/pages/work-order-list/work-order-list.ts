import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { WorkOrderService } from '../../services/work-order';
import { WorkOrder, WorkOrderStatus } from '../../work-order.model';

@Component({
  selector: 'app-work-order-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './work-order-list.html',
  styleUrl: './work-order-list.scss',
})
export class WorkOrderList implements OnInit {
  workOrders = signal<WorkOrder[]>([]);
  loading = signal(true);
  errorMessage = signal<string | null>(null);

  constructor(private workOrderService: WorkOrderService) {}

  ngOnInit(): void {
    this.loadWorkOrders();
  }

  loadWorkOrders(): void {
    this.loading.set(true);
    this.errorMessage.set(null);
    this.workOrderService.getAll().subscribe({
      next: (orders) => {
        this.workOrders.set(orders);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Work order load error:', err);
        this.errorMessage.set('Failed to load work orders.');
        this.loading.set(false);
      },
    });
  }

  statusLabel(status: WorkOrderStatus): string {
    return WorkOrderStatus[status] ?? 'Unknown';
  }

  statusClass(status: WorkOrderStatus): string {
    return WorkOrderStatus[status]?.toLowerCase() ?? '';
  }

  advanceStatus(order: WorkOrder): void {
    const next = order.status === WorkOrderStatus.Open
      ? WorkOrderStatus.InProgress
      : WorkOrderStatus.Closed;

    this.workOrderService.update(order.id, {
      id: order.id,
      title: order.title,
      description: order.description,
      assignedTo: order.assignedTo,
      status: next,
      dueDate: order.dueDate,
    }).subscribe({
      next: () => this.loadWorkOrders(),
      error: () => this.errorMessage.set('Failed to update status.'),
    });
  }

  deleteOrder(id: number): void {
    if (!confirm('Delete this work order?')) return;
    this.workOrderService.delete(id).subscribe({
      next: () => this.loadWorkOrders(),
      error: () => this.errorMessage.set('Failed to delete work order.'),
    });
  }
}