export interface OperationResult {
  success: boolean;
  message: string;
  errors: string[];
}

export interface FilterAllAuditByDateQuery {
  startDate: string;
  endDate: string;
  pageNumber: number;
  pageSize: number;
}

export interface AuthenticationRequest {
  email: string;
  password: string;
}

export interface NewUser {
  email: string;
  firstName: string;
  lastName: string;
  role: string;
}

export interface ChangePersonRoleCommand {
  personId: number;
  role: string;
}

