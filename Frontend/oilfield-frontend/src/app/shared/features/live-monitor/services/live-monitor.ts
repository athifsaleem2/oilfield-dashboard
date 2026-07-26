import { Injectable, Inject, PLATFORM_ID, NgZone } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { SensorReading } from '../sensor-reading.model';
import { Alert } from '../../alerts/alert.model';

@Injectable({
  providedIn: 'root',
})
export class LiveMonitorService {
  private connection: any = null;
  private readingsSubject = new Subject<SensorReading[]>();
  private alertsSubject = new Subject<Alert[]>();
  private isBrowser: boolean;

  readings$: Observable<SensorReading[]> = this.readingsSubject.asObservable();
  alerts$: Observable<Alert[]> = this.alertsSubject.asObservable();

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private ngZone: NgZone
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  async connect(): Promise<void> {
    if (!this.isBrowser || this.connection) return;

    const signalR = await import('@microsoft/signalr');

    const hubUrl = environment.apiUrl.replace('/api', '') + '/hubs/monitor';

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .build();

    this.connection.on('ReceiveSensorReadings', (rawReadings: any[]) => {
      const readings: SensorReading[] = (rawReadings || []).map((r: any) => ({
        id: r.id ?? r.Id ?? 0,
        wellId: r.wellId ?? r.WellId ?? 0,
        pressure: r.pressure ?? r.Pressure ?? 0,
        temperature: r.temperature ?? r.Temperature ?? 0,
        flowRate: r.flowRate ?? r.FlowRate ?? 0,
        timestamp: r.timestamp ?? r.Timestamp ?? new Date().toISOString(),
      }));
      console.log('Received & normalized sensor readings:', readings);
      this.ngZone.run(() => {
        this.readingsSubject.next(readings);
      });
    });

    this.connection.on('ReceiveAlerts', (rawAlerts: any[]) => {
      const alerts: Alert[] = (rawAlerts || []).map((a: any) => {
        const wellId = a.wellId ?? a.WellId ?? 0;
        return {
          id: a.id ?? a.Id ?? 0,
          wellId,
          wellName: a.wellName ?? a.WellName ?? `Well #${wellId}`,
          metric: a.metric ?? a.Metric ?? '',
          value: a.value ?? a.Value ?? 0,
          threshold: a.threshold ?? a.Threshold ?? 0,
          message: a.message ?? a.Message ?? '',
          isResolved: a.isResolved ?? a.IsResolved ?? false,
          createdAt: a.createdAt ?? a.CreatedAt ?? new Date().toISOString(),
        };
      });
      console.log('Received & normalized alerts:', alerts);
      this.ngZone.run(() => {
        this.alertsSubject.next(alerts);
      });
    });

    try {
      await this.connection.start();
    } catch (err) {
      console.error('SignalR connection failed:', err);
    }
  }

  disconnect(): void {
    this.connection?.stop();
    this.connection = null;
  }
}