export interface AccountTypeDTO {
  id: number;
  code: string;
  name: string;
  description: string;
  accountSubCategoryId: number;
  accountSubCategoryName: string;
  normalBalance: string;
  isActive: boolean;
  displayOrder: number;
}

export interface CreateAccountTypeCommand {
  code: string;
  name: string;
  description: string;
  accountSubCategoryId: number;
  normalBalance: NormalBalance;
  displayOrder: number;
}

export interface UpdateAccountTypeCommand {
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
