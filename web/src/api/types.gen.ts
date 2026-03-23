// AUTO-GENERATED from docs/openapi.yaml — run `npm run generate-types` to regenerate.
// Hand-authored convenience aliases appended at bottom.

export interface BaseObject {
  updatedAt?: string;
  updatedBy?: string | null;
  isActive?: boolean;
}

export interface DrugCategory extends BaseObject {
  id?: string;
  name?: string | null;
  description?: string | null;
}

export interface Drug extends BaseObject {
  id?: string;
  name?: string | null;
  genericName?: string | null;
  manufacturerName?: string | null;
  categoryId?: string;
  unitOfMeasure?: string | null;
  reorderLevel?: number;
  brandName?: string | null;
  dosageForm?: string | null;
  strength?: string | null;
  description?: string | null;
  drugLicenseNumber?: string | null;
  approvalDate?: string | null;
  scheduleCategory?: string | null;
  prescriptionRequired?: boolean;
  hsnCode?: string | null;
  gstSlab?: number;
  composition?: string | null;
  mrp?: number;
}

export interface Vendor extends BaseObject {
  id?: string;
  name?: string | null;
  contactPerson?: string | null;
  phone?: string | null;
  email?: string | null;
  address?: string | null;
  gstNumber?: string | null;
  drugLicenseNumber?: string | null;
  creditTermsDays?: number;
  creditLimit?: number;
  outstandingBalance?: number;
}

export interface AppUser extends BaseObject {
  id?: string;
  username?: string | null;
  fullName?: string | null;
  email?: string | null;
  phone?: string | null;
  passwordHash?: string | null;
  isLocked?: boolean;
  lastLoginAt?: string | null;
}

export interface Role extends BaseObject {
  id?: string;
  name?: string | null;
  description?: string | null;
}

export interface Branch extends BaseObject {
  id?: string;
  name?: string | null;
  address?: string | null;
  phone?: string | null;
  gstin?: string | null;
  drugLicenseNumber?: string | null;
  isHeadOffice?: boolean;
}

export interface Patient extends BaseObject {
  id?: string;
  name?: string | null;
  phone?: string | null;
  email?: string | null;
  address?: string | null;
  age?: number | null;
  gstin?: string | null;
  creditLimit?: number;
}

export interface Prescription extends BaseObject {
  id?: string;
  patientId?: string;
  prescriptionDate?: string | null;
  doctorName?: string | null;
  doctorRegistrationNumber?: string | null;
  patientAge?: number | null;
  notes?: string | null;
}

export interface CustomerInvoice extends BaseObject {
  id?: string;
  patientId?: string | null;
  prescriptionId?: string | null;
  invoiceDate?: string;
  invoiceNumber?: string | null;
  totalAmount?: number;
  totalDiscount?: number;
  totalCgst?: number;
  totalSgst?: number;
  totalIgst?: number;
  paymentMode?: string | null;
  branchId?: string | null;
  billedBy?: string | null;
  pharmacyGstin?: string | null;
  patientGstin?: string | null;
}

export interface PurchaseOrder extends BaseObject {
  id?: string;
  vendorId?: string;
  poNumber?: string | null;
  orderDate?: string;
  status?: string | null;
  totalAmount?: number;
  approvedBy?: string | null;
  approvedAt?: string | null;
  branchId?: string | null;
}

export interface PaymentLedger extends BaseObject {
  id?: string;
  entityType?: string | null;
  entityId?: string;
  transactionDate?: string;
  transactionType?: string | null;
  amount?: number;
  referenceNumber?: string | null;
  notes?: string | null;
}

export interface AuditLog extends BaseObject {
  id?: string;
  drugId?: string;
  drugName?: string | null;
  scheduleCategory?: string | null;
  customerInvoiceId?: string;
  prescriptionId?: string | null;
  patientId?: string | null;
  quantityDispensed?: number;
  performedBy?: string | null;
  performedAt?: string;
  retentionUntil?: string;
}
