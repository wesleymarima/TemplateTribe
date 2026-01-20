export interface ExchangeRateDTO {
  id: number;
  currencyId: number;
  currencyCode: string;
  currencyName: string;
  toCurrencyCode: string;
  rate: number;
  effectiveDate: string;
  endDate: string | null;
  isActive: boolean;
  companyId: number;
  companyName: string;
  created: string;
  createdBy: string | null;
}

export interface CreateExchangeRateCommand {
  currencyId: number;
  toCurrencyCode: string;
  rate: number;
  effectiveDate: string;
  endDate: string | null;
  companyId: number;
}

export interface UpdateExchangeRateCommand {
  id: number;
  rate: number;
  effectiveDate: string;
  endDate: string | null;
  isActive: boolean;
}

