import { Routes } from '@angular/router';
import { WellList } from './shared/features/wells/pages/well-list/well-list';
import { Dashboard } from './shared/features/live-monitor/pages/dashboard/dashboard';
import { AlertList } from './shared/features/alerts/pages/alert-list/alert-list';
import { WorkOrderList } from './shared/features/work-orders/pages/work-order-list/work-order-list';

export const routes: Routes = [
  {
    path: 'wells',
    component: WellList
  },
  {
    path: 'monitor',
    component: Dashboard
  },
  {
    path: 'alerts',
    component: AlertList
  },
  {
    path: 'work-orders',
    component: WorkOrderList
  },
  {
    path: '',
    redirectTo: 'monitor',
    pathMatch: 'full'
  }
];