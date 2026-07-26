import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // This is a live-data dashboard — all routes are rendered client-side.
  // The SSR server cannot reach the HTTPS backend (self-signed cert), and
  // real-time SignalR data cannot be prerendered anyway.
  { path: '**', renderMode: RenderMode.Client },
];
