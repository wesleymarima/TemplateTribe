export interface AccountDTO {
  id: number;
  accountCode: string;
  accountName: string;
  description: string;
  accountTypeId: number;
  accountTypeName: string;
  parentAccountId: number | null;
  parentAccountName: string | null;
  companyId: number;
  companyName: string;
  level: number;
  isActive: boolean;
  allowDirectPosting: boolean;
  currentBalance: number;
  lastTransactionDate: Date | null;
}

export interface AccountDetailDTO {
  id: number;
  accountCode: string;
  accountName: string;
  description: string;
  accountTypeId: number;
  accountTypeName: string;
  parentAccountId: number | null;
  parentAccountName: string | null;
  companyId: number;
  companyName: string;
  currencyId: number | null;
  currencyCode: string | null;
  level: number;
  isActive: boolean;
  isSystemAccount: boolean;
  allowDirectPosting: boolean;
  requiresCostCenter: boolean;
  requiresDepartment: boolean;
  requiresBranch: boolean;
  openingBalance: number;
  openingBalanceDate: Date | null;
  currentBalance: number;
  lastTransactionDate: Date | null;
  lastTransactionSequence: number | null;
  created: Date;
  createdBy: string | null;
  lastModified: Date;
  lastModifiedBy: string | null;
}

export interface CreateAccountCommand {
  accountCode: string;
  accountName: string;
  description: string;
  accountTypeId: number;
  parentAccountId: number | null;
  companyId: number;
  currencyId: number | null;
  level: number;
  allowDirectPosting: boolean;
  requiresCostCenter: boolean;
  requiresDepartment: boolean;
  requiresBranch: boolean;
  openingBalance: number;
  openingBalanceDate: Date | null;
}

export interface UpdateAccountCommand {
  id: number;
  accountName: string;
  description: string;
  isActive: boolean;
  allowDirectPosting: boolean;
  requiresCostCenter: boolean;
  requiresDepartment: boolean;
  requiresBranch: boolean;
}

export interface SetOpeningBalanceCommand {
  accountId: number;
  openingBalance: number;
  openingBalanceDate: Date;
}

export interface AccountTransactionDTO {
  id: number;
  sequenceNumber: number;
  transactionDate: Date;
  postingDate: Date;
  description: string;
  referenceNumber: string;
  debitAmount: number;
  creditAmount: number;
  runningBalance: number;
  costCenterName: string | null;
  departmentName: string | null;
  branchName: string | null;
  isReversed: boolean;
}

export interface AccountLedgerResponse {
  accountId: number;
  accountCode: string;
  accountName: string;
  openingBalance: number;
  closingBalance: number;
  transactions: AccountTransactionDTO[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}
