import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { SensorReading } from '../sensor-reading.model';

@Injectable({
  providedIn: 'root',
})
export class LiveMonitorService {
  private connection: any = null;
  private readingsSubject = new Subject<SensorReading[]>();
  private isBrowser: boolean;

  readings$: Observable<SensorReading[]> = this.readingsSubject.asObservable();

  constructor(@Inject(PLATFORM_ID) platformId: Object) {
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

    this.connection.on('ReceiveSensorReadings', (readings: SensorReading[]) => {
      this.readingsSubject.next(readings);
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