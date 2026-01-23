export interface AccountSubCategoryDTO {
  id: number;
  code: string;
  name: string;
  description: string;
  accountCategoryId: number;
  accountCategoryName: string;
  normalBalance: string;
  isActive: boolean;
  displayOrder: number;
}

export interface CreateAccountSubCategoryCommand {
  code: string;
  name: string;
  description: string;
  accountCategoryId: number;
  normalBalance: NormalBalance;
  displayOrder: number;
}

export interface UpdateAccountSubCategoryCommand {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
  displayOrder: number;
}

export enum NormalBalance {
  Debit = 1,
  Credit = 2
}
