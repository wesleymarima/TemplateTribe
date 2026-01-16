export interface BranchDTO {
  id: number;
  name: string;
  code: string;
  city: string;
  branchType: string;
  isHeadquarters: boolean;
  isActive: boolean;
  companyId: number;
  companyName: string;
  personsCount: number;
}

export interface BranchDetailDTO {
  id: number;
  name: string;
  code: string;
  email: string;
  phone: string;
  description: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  branchType: string;
  isHeadquarters: boolean;
  isActive: boolean;
  companyId: number;
  companyName: string;
  personsCount: number;
}

export interface CreateBranchCommand {
  name: string;
  code: string;
  email: string;
  phone: string;
  description: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  branchType: string;
  isHeadquarters: boolean;
  businessHours: string;
  companyId: number;
  managerId: number;
}

export interface UpdateBranchCommand {
  id: number;
  name: string;
  code: string;
  email: string;
  phone: string;
  description: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  branchType: string;
  isHeadquarters: boolean;
  isActive: boolean;
  businessHours: string;
  managerId: number;
}

