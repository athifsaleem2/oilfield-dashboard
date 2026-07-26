import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertService } from '../../services/alert';
import { Alert } from '../../alert.model';

@Component({
  selector: 'app-alert-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert-list.html',
  styleUrl: './alert-list.scss',
})
export class AlertList implements OnInit {
  alerts = signal<Alert[]>([]);
  loading = signal(true);
  errorMessage = signal<string | null>(null);
  activeCount = computed(() => this.alerts().filter((a) => !a.isResolved).length);

  constructor(private alertService: AlertService) {}

  ngOnInit(): void {
    this.loadAlerts();
  }

  loadAlerts(): void {
    this.loading.set(true);
    this.errorMessage.set(null);
    this.alertService.getAll().subscribe({
      next: (alerts) => {
        this.alerts.set(alerts);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Alert load error:', err);
        this.errorMessage.set('Failed to load alerts.');
        this.loading.set(false);
      },
    });
  }

  resolve(id: number): void {
    this.alertService.resolve(id).subscribe({
      next: () => this.loadAlerts(),
      error: () => this.errorMessage.set('Failed to resolve alert.'),
    });
  }
}