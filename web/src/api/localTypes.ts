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

export interface Drug {
  id: string;
  name: string | null;
  genericName: string | null;
  manufacturerName: string | null;
  categoryId: string;
  unitOfMeasure: string | null;
  reorderLevel: number;
  brandName: string | null;
  dosageForm: string | null;
  strength: string | null;
  description: string | null;
  scheduleCategory: string | null;
  prescriptionRequired: boolean;
  hsnCode: string | null;
  gstSlab: number;
  composition: string | null;
  mrp: number;
  isActive: boolean;
}

export interface DrugFilter {
  name?: string;
  genericName?: string;
  categoryId?: string;
  composition?: string;
}

export interface DrugCategory {
  id: string;
  name: string | null;
  description: string | null;
  isActive: boolean;
}

export interface Vendor {
  id: string;
  name: string | null;
  contactPerson: string | null;
  phone: string | null;
  email: string | null;
  address: string | null;
  gstNumber: string | null;
  drugLicenseNumber: string | null;
  creditTermsDays: number;
  creditLimit: number;
  outstandingBalance: number;
  isActive: boolean;
}

export interface VendorFilter {
  name?: string;
}

export interface PurchaseOrder {
  id: string;
  vendorId: string;
  orderDate: string;
  status: string | null;
  notes: string | null;
  totalAmount: number;
  quotationId: string | null;
  poNumber: string | null;
  approvedBy: string | null;
  approvedAt: string | null;
  parentPurchaseOrderId: string | null;
  branchId: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface PurchaseOrderFilter {
  vendorId?: string;
  status?: string;
  dateFrom?: string;
  dateTo?: string;
}

export interface PurchaseOrderItem {
  id: string;
  purchaseOrderId: string;
  drugId: string;
  quantityOrdered: number;
  quantityReceived: number;
  unitPrice: number;
  batchNumber: string | null;
  expirationDate: string | null;
  isActive: boolean;
}

export interface ExpiryRecord {
  id: string;
  drugInventoryId: string;
  detectedAt: string;
  expirationDate: string;
  quantityAffected: number;
  status: string | null;
  initiatedBy: string | null;
  approvedBy: string | null;
  approvedAt: string | null;
  quarantineRackId: string | null;
  notes: string | null;
  isActive: boolean;
}

export interface ExpiryRecordFilter {
  drugInventoryId?: string;
  status?: string;
  dateFrom?: string;
  dateTo?: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: import('./types.gen').AppUser;
}

export interface DailySalesReport {
  date: string;
  invoiceCount: number;
  subTotal: number;
  totalDiscount: number;
  totalCgst: number;
  totalSgst: number;
  totalIgst: number;
  gstAmount: number;
  netAmount: number;
}

// Branch — uses backend field names (pharmacyLicenseNumber, not drugLicenseNumber)
// types.gen.ts version has wrong field names.
export interface Branch {
  id: string;
  name: string | null;
  address: string | null;
  gstin: string | null;
  pharmacyLicenseNumber: string | null;
  phone: string | null;
  email: string | null;
  managerUserId: string | null;
  updatedAt: string;
  updatedBy: string | null;
  isActive: boolean;
}

// CustomerInvoice — uses backend field names (subTotal, discountAmount, paymentMethod)
// types.gen.ts version is outdated; import from here instead.
export interface CustomerInvoice {
  id: string;
  patientId: string | null;
  prescriptionId: string | null;
  invoiceDate: string;
  invoiceNumber: string | null;
  subTotal: number;
  discountAmount: number;
  gstAmount: number;
  netAmount: number;
  paymentMethod: string | null;
  status: string | null;
  notes: string | null;
  totalCgst: number;
  totalSgst: number;
  totalIgst: number;
  branchId: string | null;
  billedBy: string | null;
  pharmacyGstin: string | null;
  patientGstin: string | null;
  updatedAt: string;
  updatedBy: string | null;
  isActive: boolean;
}

export interface CustomerInvoiceItem {
  id: string;
  invoiceId: string;
  drugId: string;
  batchNumber: string | null;
  quantity: number;
  unitPrice: number;
  discountPercent: number;
  gstRate: number;
  amount: number;
  hsnCode: string | null;
  taxableValue: number;
  cgstAmount: number;
  sgstAmount: number;
  igstAmount: number;
  isActive: boolean;
}
