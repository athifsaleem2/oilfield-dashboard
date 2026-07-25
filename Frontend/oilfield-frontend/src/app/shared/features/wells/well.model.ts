export enum WellStatus {
  Active = 0,
  Inactive = 1,
  Maintenance = 2,
}

export interface Well {
  id: number;
  name: string;
  location: string;
  status: WellStatus;
  latitude: number;
  longitude: number;
}

export interface CreateWellRequest {
  name: string;
  location: string;
  status: WellStatus;
  latitude: number;
  longitude: number;
}

export interface UpdateWellRequest extends CreateWellRequest {
  id: number;
}