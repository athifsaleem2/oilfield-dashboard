import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar {
  navItems = [
    { path: '/monitor', label: 'Monitor', icon: '◉' },
    { path: '/wells', label: 'Wells', icon: '⬢' },
    { path: '/alerts', label: 'Alerts', icon: '⚠' },
    { path: '/work-orders', label: 'Work Orders', icon: '☰' },
  ];
}