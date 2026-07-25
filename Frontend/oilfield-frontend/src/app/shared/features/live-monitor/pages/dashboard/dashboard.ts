import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LiveMonitorService } from '../../services/live-monitor';
import { SensorReading } from '../../sensor-reading.model';

const MAX_READINGS_PER_WELL = 20;

type MetricKey = 'pressure' | 'temperature' | 'flowRate';

interface Thresholds {
  warnLow: number;
  warnHigh: number;
  unit: string;
}

const THRESHOLDS: Record<MetricKey, Thresholds> = {
  pressure: { warnLow: 2050, warnHigh: 2450, unit: 'psi' },
  temperature: { warnLow: 160, warnHigh: 190, unit: '°F' },
  flowRate: { warnLow: 150, warnHigh: 450, unit: 'bbl/day' },
};

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit, OnDestroy {
  wellReadings = new Map<number, SensorReading[]>();
  metricKeys: MetricKey[] = ['pressure', 'temperature', 'flowRate'];
  private subscription: any;

  constructor(
    private liveMonitor: LiveMonitorService,
    private cdr: ChangeDetectorRef
  ) {}

  async ngOnInit(): Promise<void> {
    await this.liveMonitor.connect();

    this.subscription = this.liveMonitor.readings$.subscribe((newReadings) => {
      for (const reading of newReadings) {
        const existing = this.wellReadings.get(reading.wellId) ?? [];
        const updated = [...existing, reading].slice(-MAX_READINGS_PER_WELL);
        this.wellReadings.set(reading.wellId, updated);
      }
      this.cdr.detectChanges();
    });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
    this.liveMonitor.disconnect();
  }

  get wellIds(): number[] {
    return Array.from(this.wellReadings.keys()).sort((a, b) => a - b);
  }

  latest(wellId: number): SensorReading | undefined {
    const readings = this.wellReadings.get(wellId);
    return readings?.[readings.length - 1];
  }

  unit(key: MetricKey): string {
    return THRESHOLDS[key].unit;
  }

  label(key: MetricKey): string {
    return key === 'flowRate' ? 'Flow Rate' : key.charAt(0).toUpperCase() + key.slice(1);
  }

  status(wellId: number, key: MetricKey): 'normal' | 'warning' | 'critical' {
    const reading = this.latest(wellId);
    if (!reading) return 'normal';
    const value = reading[key];
    const { warnLow, warnHigh } = THRESHOLDS[key];

    if (value < warnLow * 0.95 || value > warnHigh * 1.05) return 'critical';
    if (value < warnLow || value > warnHigh) return 'warning';
    return 'normal';
  }

  // Smooth curve using quadratic bezier midpoint smoothing
  private smoothPath(points: { x: number; y: number }[]): string {
    if (points.length < 2) return '';
    let d = `M ${points[0].x},${points[0].y}`;
    for (let i = 0; i < points.length - 1; i++) {
      const mx = (points[i].x + points[i + 1].x) / 2;
      const my = (points[i].y + points[i + 1].y) / 2;
      d += ` Q ${points[i].x},${points[i].y} ${mx},${my}`;
    }
    const last = points[points.length - 1];
    d += ` T ${last.x},${last.y}`;
    return d;
  }

  private computePoints(wellId: number, key: MetricKey): { x: number; y: number }[] {
    const readings = this.wellReadings.get(wellId) ?? [];
    if (readings.length < 2) return [];

    const values = readings.map((r) => r[key]);
    const min = Math.min(...values);
    const max = Math.max(...values);
    const range = max - min || 1;

    const width = 220;
    const height = 52;
    const padding = 6;
    const step = width / (readings.length - 1);

    return values.map((v, i) => ({
      x: i * step,
      y: padding + (height - padding * 2) - ((v - min) / range) * (height - padding * 2),
    }));
  }

  sparklinePath(wellId: number, key: MetricKey): string {
    return this.smoothPath(this.computePoints(wellId, key));
  }

  areaPath(wellId: number, key: MetricKey): string {
    const points = this.computePoints(wellId, key);
    if (points.length < 2) return '';
    const path = this.smoothPath(points);
    const last = points[points.length - 1];
    const first = points[0];
    return `${path} L ${last.x},52 L ${first.x},52 Z`;
  }

  lastPoint(wellId: number, key: MetricKey): { x: number; y: number } | null {
    const points = this.computePoints(wellId, key);
    return points.length ? points[points.length - 1] : null;
  }
}