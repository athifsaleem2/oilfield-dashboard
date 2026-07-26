export interface Alert {
  id: number;
  wellId: number;
  wellName: string;
  metric: string;
  value: number;
  threshold: number;
  message: string;
  isResolved: boolean;
  createdAt: string;
}