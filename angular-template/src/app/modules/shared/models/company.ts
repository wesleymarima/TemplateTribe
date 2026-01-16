export interface CompanyDTO {
  id: number;
  name: string;
  legalName: string;
  taxId: string;
  email: string;
  phone: string;
  city: string;
  country: string;
  isActive: boolean;
  branchesCount: number;
}

export interface CompanyDetailDTO {
  id: number;
  name: string;
  legalName: string;
  taxId: string;
  registrationNumber: string;
  email: string;
  phone: string;
  website: string;
  logoUrl: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  state: string;
  country: string;
  currencyId: number;
  currencyCode: string;
  fiscalYearStartMonth: number;
  isActive: boolean;
  branches: BranchSummaryDTO[];
}

export interface BranchSummaryDTO {
  id: number;
  name: string;
  code: string;
  city: string;
  branchType: string;
  isActive: boolean;
}

export interface CreateCompanyCommand {
  name: string;
  legalName: string;
  taxId: string;
  registrationNumber: string;
  email: string;
  phone: string;
  website: string;
  logoUrl: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  currencyId: number;
  fiscalYearStartMonth: number;
}

export interface UpdateCompanyCommand {
  id: number;
  name: string;
  legalName: string;
  taxId: string;
  registrationNumber: string;
  email: string;
  phone: string;
  website: string;
  logoUrl: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  currencyId: number;
  fiscalYearStartMonth: number;
  isActive: boolean;
}

