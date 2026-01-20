export interface CurrencyDTO {
  id: number;
  code: string;
  name: string;
  symbol: string;
  decimalPlaces: number;
  isActive: boolean;
  created: string;
  createdBy: string | null;
  lastModified: string;
  lastModifiedBy: string | null;
}

export interface CreateCurrencyCommand {
  code: string;
  name: string;
  symbol: string;
  decimalPlaces: number;
}

export interface UpdateCurrencyCommand {
  id: number;
  code: string;
  name: string;
  symbol: string;
  decimalPlaces: number;
  isActive: boolean;
}

