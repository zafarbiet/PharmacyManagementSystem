// Manually maintained types for entities not yet in openapi.yaml.
// Remove each entry once the backend adds it to the spec.

export interface DrugInventory {
  id: string;
  drugId: string;
  batchNumber: string | null;
  expirationDate: string;
  quantityInStock: number;
  storageConditions: string | null;
  updatedAt: string;
  updatedBy: string | null;
  isActive: boolean;
}

export interface DrugInventoryFilter {
  drugId?: string;
  batchNumber?: string;
  expiresBeforeDate?: string;
  dateFrom?: string;
  dateTo?: string;
}
