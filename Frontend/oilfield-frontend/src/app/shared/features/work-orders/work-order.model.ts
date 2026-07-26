export enum WorkOrderStatus {
  Open = 0,
  InProgress = 1,
  Closed = 2,
}

export interface WorkOrder {
  id: number;
  wellId: number;
  wellName: string;
  title: string;
  description: string;
  assignedTo: string;
  status: WorkOrderStatus;
  dueDate: string;
  createdAt: string;
}

export interface CreateWorkOrderRequest {
  wellId: number;
  title: string;
  description: string;
  assignedTo: string;
  dueDate: string;
}

export interface UpdateWorkOrderRequest {
  id: number;
  title: string;
  description: string;
  assignedTo: string;
  status: WorkOrderStatus;
  dueDate: string;
}