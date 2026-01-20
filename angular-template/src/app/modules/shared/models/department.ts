export interface DepartmentDTO {
  id: number;
  code: string;
  name: string;
  description: string;
  managerId: string | null;
  parentDepartmentId: number | null;
  parentDepartmentName: string | null;
  isActive: boolean;
  companyId: number;
  companyName: string;
  childCount: number;
  created: string;
  createdBy: string | null;
  lastModified: string;
  lastModifiedBy: string | null;
}

export interface CreateDepartmentCommand {
  code: string;
  name: string;
  description: string;
  managerId: string | null;
  parentDepartmentId: number | null;
  companyId: number;
}

export interface UpdateDepartmentCommand {
  id: number;
  code: string;
  name: string;
  description: string;
  managerId: string | null;
  parentDepartmentId: number | null;
  isActive: boolean;
}

