export enum PeriodStatus {
  Open = 1,
  Closed = 2
}

export interface FinancialPeriodDTO {
  id: number;
  name: string;
  periodNumber: number;
  fiscalYear: number;
  startDate: string;
  endDate: string;
  status: PeriodStatus;
  statusName: string;
  closedBy: string | null;
  closedDate: string | null;
  reopenedBy: string | null;
  reopenedDate: string | null;
  reopenReason: string | null;
  companyId: number;
  companyName: string;
  created: string;
  createdBy: string | null;
  lastModified: string;
  lastModifiedBy: string | null;
}

export interface CreateFinancialPeriodCommand {
  name: string;
  periodNumber: number;
  fiscalYear: number;
  startDate: string;
  endDate: string;
  companyId: number;
}

export interface ReopenFinancialPeriodCommand {
  id: number;
  reopenReason: string;
}

