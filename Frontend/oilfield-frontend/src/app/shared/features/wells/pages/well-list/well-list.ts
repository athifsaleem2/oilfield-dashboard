import { Component, OnInit, AfterViewInit, OnDestroy, Inject, PLATFORM_ID, signal } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterLink } from '@angular/router';
import { WellService } from '../../services/well';
import { Well, WellStatus } from '../../well.model';
import type * as LeafletType from 'leaflet';

@Component({
  selector: 'app-well-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './well-list.html',
  styleUrl: './well-list.scss',
})
export class WellList implements OnInit, AfterViewInit, OnDestroy {
  wells = signal<Well[]>([]);
  loading = signal(true);
  errorMessage = signal<string | null>(null);

  private map: LeafletType.Map | null = null;
  private markers: LeafletType.Marker[] = [];
  private L: typeof LeafletType | null = null;
  private isBrowser: boolean;

  constructor(
    private wellService: WellService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.loadWells();
  }

  async ngAfterViewInit(): Promise<void> {
    if (!this.isBrowser) return;

    this.L = await import('leaflet');

    // Fix default icon paths for bundled Leaflet
    delete (this.L.Icon.Default.prototype as any)._getIconUrl;
    this.L.Icon.Default.mergeOptions({
      iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
      iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
      shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
    });

    this.map = this.L.map('well-map').setView([29.3759, 47.9774], 10);
    this.L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors',
    }).addTo(this.map);

    this.plotMarkers();
  }

  ngOnDestroy(): void {
    this.map?.remove();
  }

  loadWells(): void {
    this.loading.set(true);
    this.errorMessage.set(null);
    this.wellService.getAll().subscribe({
      next: (wells) => {
        this.wells.set(wells);
        this.loading.set(false);
        this.plotMarkers();
      },
      error: (err) => {
        console.error('Well list load error:', err);
        this.errorMessage.set('Failed to load wells.');
        this.loading.set(false);
      },
    });
  }

  plotMarkers(): void {
    if (!this.map || !this.L) return;

    this.markers.forEach((m) => m.remove());
    this.markers = [];

    const bounds: [number, number][] = [];

    this.wells().forEach((well) => {
      const isMaintenance = well.status === WellStatus.Maintenance;
      const isInactive = well.status === WellStatus.Inactive;
      const color = isMaintenance ? '#f0a93b' : isInactive ? '#ef4444' : '#22c55e';

      const customIcon = this.L!.divIcon({
        className: 'well-map-marker',
        html: `<div style="
          width: 22px;
          height: 22px;
          border-radius: 50%;
          background: ${color};
          border: 2px solid #0f172a;
          box-shadow: 0 0 10px ${color};
          display: flex;
          align-items: center;
          justify-content: center;
          color: #0f172a;
          font-size: 10px;
          font-weight: bold;
        ">◈</div>`,
        iconSize: [22, 22],
        iconAnchor: [11, 11],
        popupAnchor: [0, -11],
      });

      if (well.latitude && well.longitude) {
        bounds.push([well.latitude, well.longitude]);
        const marker = this.L!.marker([well.latitude, well.longitude], { icon: customIcon })
          .addTo(this.map!)
          .bindPopup(`<strong>${well.name}</strong><br/>${well.location}<br/>Status: ${this.statusLabel(well.status)}`);
        this.markers.push(marker);
      }
    });

    if (bounds.length > 0) {
      this.map.fitBounds(bounds, { padding: [30, 30] });
    }
  }

  deleteWell(id: number): void {
    if (!confirm('Delete this well?')) return;
    this.wellService.delete(id).subscribe({
      next: () => this.loadWells(),
      error: () => this.errorMessage.set('Failed to delete well.'),
    });
  }

  statusLabel(status: WellStatus): string {
    return WellStatus[status] ?? 'Unknown';
  }

  statusClass(status: WellStatus): string {
    return WellStatus[status]?.toLowerCase() ?? '';
  }
}