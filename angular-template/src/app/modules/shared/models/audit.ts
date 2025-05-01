export interface Audit {
  userID: string;
  type: string;
  tableName: string;
  dateTime: Date;
  oldValues: string;
  newValues: string;
  affectedColumns: string;
  primaryKey: string;
  created: Date;
  createdBy: string;
  lastModified: Date;
  lastModifiedBy: string;
}
