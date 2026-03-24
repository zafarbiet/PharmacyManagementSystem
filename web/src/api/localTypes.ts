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

export interface Patient {
  id: string;
  name: string | null;
  contactNumber: string | null;
  email: string | null;
  address: string | null;
  age: number | null;
  gstin: string | null;
  creditBalance: number;
  isSubscriber: boolean;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface Prescription {
  id: string;
  patientId: string;
  prescribingDoctor: string | null;
  doctorRegistrationNumber: string | null;
  prescriptionDate: string;
  patientAge: number | null;
  notes: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface PrescriptionItem {
  id: string;
  prescriptionId: string;
  drugId: string;
  dosage: string | null;
  quantity: number;
  instructions: string | null;
  isActive: boolean;
}

export interface QuotationRequest {
  id: string;
  requestDate: string;
  requiredByDate: string | null;
  status: string | null;
  notes: string | null;
  requestedBy: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface QuotationRequestItem {
  id: string;
  quotationRequestId: string;
  drugId: string;
  quantityRequired: number;
  notes: string | null;
  isActive: boolean;
}

export interface Quotation {
  id: string;
  quotationRequestId: string;
  vendorId: string;
  quotationDate: string;
  validUntil: string | null;
  status: string | null;
  totalAmount: number;
  notes: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface QuotationItem {
  id: string;
  quotationId: string;
  drugId: string;
  quantityOffered: number;
  unitPrice: number;
  discountPercent: number;
  gstRate: number;
  totalAmount: number;
  notes: string | null;
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

export interface Role {
  id: string;
  name: string | null;
  description: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface UserRole {
  id: string;
  userId: string;
  roleId: string;
  assignedAt: string;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface DebtRecord {
  id: string;
  patientId: string;
  invoiceId: string;
  originalAmount: number;
  remainingAmount: number;
  dueDate: string | null;
  status: string | null;
  notes: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface DebtPayment {
  id: string;
  debtRecordId: string;
  paymentDate: string;
  amountPaid: number;
  paymentMethod: string | null;
  receivedBy: string | null;
  notes: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface DebtReminder {
  id: string;
  debtRecordId: string;
  sentAt: string;
  channel: string | null;
  sentBy: string | null;
  message: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}
