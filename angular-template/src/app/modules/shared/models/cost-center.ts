export interface CostCenterDTO {
  id: number;
  code: string;
  name: string;
  description: string;
  parentCostCenterId: number | null;
  parentCostCenterName: string | null;
  isActive: boolean;
  isMandatoryForExpenses: boolean;
  isMandatoryForProcurement: boolean;
  isMandatoryForJournalEntries: boolean;
  companyId: number;
  companyName: string;
  childCount: number;
  created: string;
  createdBy: string | null;
  lastModified: string;
  lastModifiedBy: string | null;
}

export interface CreateCostCenterCommand {
  code: string;
  name: string;
  description: string;
  parentCostCenterId: number | null;
  isMandatoryForExpenses: boolean;
  isMandatoryForProcurement: boolean;
  isMandatoryForJournalEntries: boolean;
  companyId: number;
}

export interface UpdateCostCenterCommand {
  id: number;
  code: string;
  name: string;
  description: string;
  parentCostCenterId: number | null;
  isActive: boolean;
  isMandatoryForExpenses: boolean;
  isMandatoryForProcurement: boolean;
  isMandatoryForJournalEntries: boolean;
}

