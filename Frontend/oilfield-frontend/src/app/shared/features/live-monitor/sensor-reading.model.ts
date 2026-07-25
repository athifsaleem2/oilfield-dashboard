export interface SensorReading {
  id: number;
  wellId: number;
  pressure: number;
  temperature: number;
  flowRate: number;
  timestamp: string;
}