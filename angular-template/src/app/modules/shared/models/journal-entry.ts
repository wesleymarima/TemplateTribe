export interface JournalEntryDTO {
  id: number;
  journalNumber: string;
  companyId: number;
  companyName: string;
  transactionDate: string;
  postingDate: string;
  description: string;
  referenceNumber: string;
  entryType: string;
  status: string;
  totalDebit: number;
  totalCredit: number;
  postedBy?: string;
  postedDate?: string;
}

export interface JournalEntryDetailDTO {
  id: number;
  journalNumber: string;
  companyId: number;
  companyName: string;
  financialPeriodId: number;
  financialPeriodName: string;
  transactionDate: string;
  postingDate: string;
  description: string;
  referenceNumber: string;
  entryType: string;
  status: string;
  totalDebit: number;
  totalCredit: number;
  postedBy?: string;
  postedDate?: string;
  approvedBy?: string;
  approvedDate?: string;
  lines: JournalEntryLineDTO[];
  created: string;
  createdBy?: string;
}

export interface JournalEntryLineDTO {
  id: number;
  lineNumber: number;
  accountId: number;
  accountCode: string;
  accountName: string;
  description: string;
  debitAmount: number;
  creditAmount: number;
  costCenterId?: number;
  costCenterName?: string;
  departmentId?: number;
  departmentName?: string;
  branchId?: number;
  branchName?: string;
  analysisCode?: string;
  memo?: string;
}

export interface CreateJournalEntryCommand {
  transactionDate: string;
  description: string;
  referenceNumber: string;
  entryType: JournalEntryType;
  lines: JournalEntryLineCommand[];
}

export interface JournalEntryLineCommand {
  lineNumber: number;
  accountId: number;
  description: string;
  debitAmount: number;
  creditAmount: number;
  costCenterId?: number;
  departmentId?: number;
  branchId?: number;
  analysisCode?: string;
  memo?: string;
}

export interface UpdateJournalEntryCommand {
  id: number;
  transactionDate: string;
  description: string;
  referenceNumber: string;
  lines: UpdateJournalEntryLineCommand[];
}

export interface UpdateJournalEntryLineCommand {
  id: number;
  lineNumber: number;
  accountId: number;
  description: string;
  debitAmount: number;
  creditAmount: number;
  costCenterId?: number;
  departmentId?: number;
  branchId?: number;
  analysisCode?: string;
  memo?: string;
}

export interface ReverseJournalEntryRequest {
  reversalDate: string;
  description: string;
}

export enum JournalEntryType {
  Manual = 1,
  Opening = 2,
  Adjustment = 3,
  Reversal = 4,
  Closing = 5,
  Recurring = 6
}

export const JournalEntryTypeLabels: Record<JournalEntryType, string> = {
  [JournalEntryType.Manual]: 'Manual',
  [JournalEntryType.Opening]: 'Opening',
  [JournalEntryType.Adjustment]: 'Adjustment',
  [JournalEntryType.Reversal]: 'Reversal',
  [JournalEntryType.Closing]: 'Closing',
  [JournalEntryType.Recurring]: 'Recurring'
};
