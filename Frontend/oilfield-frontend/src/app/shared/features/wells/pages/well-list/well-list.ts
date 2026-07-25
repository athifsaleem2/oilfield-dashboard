import { Component, OnInit, AfterViewInit, OnDestroy, Inject, PLATFORM_ID ,ChangeDetectorRef} from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterLink } from '@angular/router';
import { WellService } from '../../services/well';
import { Well, WellStatus } from '../../well.model';
import type * as LeafletType from 'leaflet';
import markerIcon2x from 'leaflet/dist/images/marker-icon-2x.png';
import markerIcon from 'leaflet/dist/images/marker-icon.png';
import markerShadow from 'leaflet/dist/images/marker-shadow.png';

@Component({
  selector: 'app-well-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './well-list.html',
  styleUrl: './well-list.scss',
})
export class WellList implements OnInit, AfterViewInit, OnDestroy {
  wells: Well[] = [];
  loading = true;
  errorMessage: string | null = null;
  private map: LeafletType.Map | null = null;
  private markers: LeafletType.Marker[] = [];
  private L: typeof LeafletType | null = null;
  private isBrowser: boolean;

 constructor(
    private wellService: WellService,
    @Inject(PLATFORM_ID) platformId: Object,
    private cdr: ChangeDetectorRef
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.loadWells();
  }

  async ngAfterViewInit(): Promise<void> {
    if (!this.isBrowser) return;

    this.L = await import('leaflet');

    this.L.Icon.Default.mergeOptions({
      iconRetinaUrl: markerIcon2x,
      iconUrl: markerIcon,
      shadowUrl: markerShadow,
    });

    this.map = this.L.map('well-map').setView([31.9973, -102.0779], 6);
    this.L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors',
    }).addTo(this.map);

    this.plotMarkers();
  }

  ngOnDestroy(): void {
    this.map?.remove();
  }

 
  loadWells(): void {
    this.loading = true;
    this.wellService.getAll().subscribe({
      next: (wells) => {
        this.wells = wells;
        this.loading = false;
        this.plotMarkers();
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load wells.';
        this.loading = false;
        this.cdr.detectChanges();
      },
    });
  }

  plotMarkers(): void {
    if (!this.map || !this.L) return;

    this.markers.forEach((m) => m.remove());
    this.markers = [];

    this.wells.forEach((well) => {
      const marker = this.L!.marker([well.latitude, well.longitude])
        .addTo(this.map!)
.bindPopup(`<strong>${well.name}</strong><br/>${well.location}<br/>Status: ${WellStatus[well.status]}`);      this.markers.push(marker);
    });
  }

  deleteWell(id: number): void {
    if (!confirm('Delete this well?')) return;
    this.wellService.delete(id).subscribe({
      next: () => this.loadWells(),
      error: () => (this.errorMessage = 'Failed to delete well.'),
    });
  }

statusLabel(status: WellStatus): string {
  return WellStatus[status];
}

statusClass(status: WellStatus): string {
  return WellStatus[status].toLowerCase();
}
}