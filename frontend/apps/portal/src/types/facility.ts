export interface FacilityDto {
  id: string;
  name: string;
  description?: string;
  ownerUnit?: string;
  type?: string;
  requiresApproval: boolean;
  capacity?: number;
  creationTime: string;
}

export interface FacilityGetListInput {
  campus?: string;
  type?: string;
  minCapacity?: number;
  maxCapacity?: number;
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
}

