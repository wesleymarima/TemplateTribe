export interface AccountCategoryDTO {
  id: number;
  code: string;
  name: string;
  description: string;
  type: string;
  normalBalance: string;
  isActive: boolean;
  displayOrder: number;
}

export interface CreateAccountCategoryCommand {
  code: string;
  name: string;
  description: string;
  type: CategoryType;
  normalBalance: NormalBalance;
  displayOrder: number;
}

export interface UpdateAccountCategoryCommand {
  id: number;
  name: string;
  description: string;
  displayOrder: number;
}

export enum CategoryType {
  Asset = 1,
  Liability = 2,
  Equity = 3,
  Revenue = 4,
  Expense = 5
}

export enum NormalBalance {
  Debit = 1,
  Credit = 2
}
