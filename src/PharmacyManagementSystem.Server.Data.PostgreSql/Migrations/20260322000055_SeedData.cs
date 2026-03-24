using FluentMigrator;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Migrations;

[Migration(20260322000055)]
public class SeedData : Migration
{
    // Fixed GUIDs for all seed rows
    private const string DrugCategoryId1 = "11111111-0001-0001-0001-000000000001";
    private const string DrugCategoryId2 = "11111111-0001-0001-0001-000000000002";
    private const string DrugId1 = "22222222-0002-0002-0002-000000000001";
    private const string DrugId2 = "22222222-0002-0002-0002-000000000002";
    private const string VendorId1 = "33333333-0003-0003-0003-000000000001";
    private const string VendorId2 = "33333333-0003-0003-0003-000000000002";
    private const string UserId1 = "44444444-0004-0004-0004-000000000001";
    private const string UserId2 = "44444444-0004-0004-0004-000000000002";
    private const string RoleId1 = "55555555-0005-0005-0005-000000000001";
    private const string RoleId2 = "55555555-0005-0005-0005-000000000002";
    private const string UserRoleId1 = "56565656-0005-0005-0005-000000000001";
    private const string UserRoleId2 = "56565656-0005-0005-0005-000000000002";
    private const string StorageZoneId1 = "66666666-0006-0006-0006-000000000001";
    private const string RackId1 = "77777777-0007-0007-0007-000000000001";
    private const string RackId2 = "77777777-0007-0007-0007-000000000002";
    private const string BranchId1 = "B0B0B0B0-0001-0001-0001-000000000001";
    private const string PatientId1 = "88888888-0008-0008-0008-000000000001";
    private const string PatientId2 = "88888888-0008-0008-0008-000000000002";
    private const string PrescriptionId1 = "99999999-0009-0009-0009-000000000001";
    private const string PrescriptionItemId1 = "9A9A9A9A-0009-0009-0009-000000000001";
    private const string DrugInventoryId1 = "AAAAAAAA-000A-000A-000A-000000000001";
    private const string DrugInventoryId2 = "AAAAAAAA-000A-000A-000A-000000000002";
    private const string DrugPricingId1 = "A1A1A1A1-000A-000A-000A-000000000001";
    private const string DrugInventoryRackId1 = "BBBBBBBB-000B-000B-000B-000000000001";
    private const string ExpiryAlertConfigId1 = "CCCCCCCC-000C-000C-000C-000000000001";
    private const string ExpiryRecordId1 = "DDDDDDDD-000D-000D-000D-000000000001";
    private const string DisposalRecordId1 = "EEEEEEEE-000E-000E-000E-000000000001";
    private const string VendorExpiryReturnId1 = "FFFFFFFF-000F-000F-000F-000000000001";
    private const string QuotationRequestId1 = "10101010-0010-0010-0010-000000000001";
    private const string QuotationRequestItemId1 = "10A10A10-0010-0010-0010-000000000001";
    private const string QuotationId1 = "11001100-0011-0011-0011-000000000001";
    private const string QuotationItemId1 = "11A011A0-0011-0011-0011-000000000001";
    private const string PurchaseOrderId1 = "12001200-0012-0012-0012-000000000001";
    private const string PurchaseOrderItemId1 = "12A012A0-0012-0012-0012-000000000001";
    private const string PaymentLedgerId1 = "1E001E00-001E-001E-001E-000000000001";
    private const string CustomerInvoiceId1 = "13001300-0013-0013-0013-000000000001";
    private const string CustomerInvoiceItemId1 = "13A013A0-0013-0013-0013-000000000001";
    private const string CustomerSubscriptionId1 = "14001400-0014-0014-0014-000000000001";
    private const string CustomerSubscriptionItemId1 = "14A014A0-0014-0014-0014-000000000001";
    private const string SubscriptionFulfillmentId1 = "15001500-0015-0015-0015-000000000001";
    private const string DebtRecordId1 = "16001600-0016-0016-0016-000000000001";
    private const string DebtPaymentId1 = "16A016A0-0016-0016-0016-000000000001";
    private const string DebtReminderId1 = "16B016B0-0016-0016-0016-000000000001";
    private const string DamageRecordId1 = "17001700-0017-0017-0017-000000000001";
    private const string DamageDisposalRecordId1 = "17A017A0-0017-0017-0017-000000000001";
    private const string DailyDiaryEntryId1 = "18001800-0018-0018-0018-000000000001";
    private const string NotificationId1 = "19001900-0019-0019-0019-000000000001";
    private const string AuditLogId1 = "1A001A00-001A-001A-001A-000000000001";

    public override void Up()
    {
        // DrugCategories
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DrugCategories WHERE Id = '{DrugCategoryId1}')
            INSERT INTO PMS.DrugCategories (Id, Name, Description, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DrugCategoryId1}', 'Antibiotics', 'Drugs used to treat bacterial infections', SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.DrugCategories WHERE Id = '{DrugCategoryId2}')
            INSERT INTO PMS.DrugCategories (Id, Name, Description, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DrugCategoryId2}', 'Analgesics', 'Pain relief medications', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Drugs (includes columns added by migration 042: HsnCode, GstSlab, Composition, Mrp)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Drugs WHERE Id = '{DrugId1}')
            INSERT INTO PMS.Drugs (Id, Name, GenericName, ManufacturerName, CategoryId, UnitOfMeasure, ReorderLevel, HsnCode, GstSlab, Composition, Mrp, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DrugId1}', 'Amoxicillin 500mg', 'Amoxicillin', 'Sun Pharma', '{DrugCategoryId1}', 'Strip', 10, '30049099', 12.00, 'Amoxicillin 500mg', 85.00, SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.Drugs WHERE Id = '{DrugId2}')
            INSERT INTO PMS.Drugs (Id, Name, GenericName, ManufacturerName, CategoryId, UnitOfMeasure, ReorderLevel, HsnCode, GstSlab, Composition, Mrp, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DrugId2}', 'Paracetamol 500mg', 'Paracetamol', 'Cipla', '{DrugCategoryId2}', 'Strip', 20, '30049041', 12.00, 'Paracetamol 500mg', 30.00, SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Vendors (includes columns added by migration 048: LicenseNumber, CreditDays)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Vendors WHERE Id = '{VendorId1}')
            INSERT INTO PMS.Vendors (Id, Name, ContactPerson, Phone, Email, Address, GstNumber, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{VendorId1}', 'Amit Pharmaceuticals', 'Amit Kumar', '9876543210', 'amit@amitpharma.com', '12 MG Road, Delhi', '07AAACA3408D1ZV', SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.Vendors WHERE Id = '{VendorId2}')
            INSERT INTO PMS.Vendors (Id, Name, ContactPerson, Phone, Email, Address, GstNumber, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{VendorId2}', 'Medi Pharma', 'Ravi Shankar', '9123456780', 'ravi@medipharma.com', '45 Linking Road, Mumbai', '27AAACM4325N1ZY', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Users
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Users WHERE Id = '{UserId1}')
            INSERT INTO PMS.Users (Id, Username, FullName, Email, Phone, PasswordHash, IsLocked, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{UserId1}', 'admin', 'System Administrator', 'admin@pharmacy.com', '9000000001', 'AQAAAAIAAYagAAAAEHashedPasswordHere==', 0, SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.Users WHERE Id = '{UserId2}')
            INSERT INTO PMS.Users (Id, Username, FullName, Email, Phone, PasswordHash, IsLocked, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{UserId2}', 'pharmacist1', 'Priya Sharma', 'priya@pharmacy.com', '9000000002', 'AQAAAAIAAYagAAAAEHashedPasswordHere==', 0, SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Roles
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Roles WHERE Id = '{RoleId1}')
            INSERT INTO PMS.Roles (Id, Name, Description, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{RoleId1}', 'Admin', 'Full system access', SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.Roles WHERE Id = '{RoleId2}')
            INSERT INTO PMS.Roles (Id, Name, Description, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{RoleId2}', 'Pharmacist', 'Dispensing and inventory access', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // UserRoles
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.UserRoles WHERE Id = '{UserRoleId1}')
            INSERT INTO PMS.UserRoles (Id, UserId, RoleId, AssignedAt, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{UserRoleId1}', '{UserId1}', '{RoleId1}', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.UserRoles WHERE Id = '{UserRoleId2}')
            INSERT INTO PMS.UserRoles (Id, UserId, RoleId, AssignedAt, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{UserRoleId2}', '{UserId2}', '{RoleId2}', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // StorageZones
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.StorageZones WHERE Id = '{StorageZoneId1}')
            INSERT INTO PMS.StorageZones (Id, Name, ZoneType, Description, TemperatureRangeMin, TemperatureRangeMax, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{StorageZoneId1}', 'Zone A - General', 'Ambient', 'General storage at room temperature', 15.00, 25.00, SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Racks
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Racks WHERE Id = '{RackId1}')
            INSERT INTO PMS.Racks (Id, StorageZoneId, Label, Description, Capacity, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{RackId1}', '{StorageZoneId1}', 'A-01', 'Rack 1 in Zone A', 100, SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.Racks WHERE Id = '{RackId2}')
            INSERT INTO PMS.Racks (Id, StorageZoneId, Label, Description, Capacity, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{RackId2}', '{StorageZoneId1}', 'A-02', 'Rack 2 in Zone A', 100, SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Branches
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Branches WHERE Id = '{BranchId1}')
            INSERT INTO PMS.Branches (Id, Name, Address, Gstin, PharmacyLicenseNumber, Phone, Email, ManagerUserId, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{BranchId1}', 'Main Branch', '1 Hospital Road, Delhi', '07AABCU9603R1ZX', 'PH/DL/2024/001', '011-12345678', 'main@pharmacy.com', '{UserId1}', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Patients (includes columns added by migration 047: Age, Gstin, CreditLimit)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Patients WHERE Id = '{PatientId1}')
            INSERT INTO PMS.Patients (Id, Name, ContactNumber, Email, Address, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{PatientId1}', 'Ramesh Kumar', '9111111111', 'ramesh@gmail.com', '5 Lajpat Nagar, Delhi', SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.Patients WHERE Id = '{PatientId2}')
            INSERT INTO PMS.Patients (Id, Name, ContactNumber, Email, Address, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{PatientId2}', 'Sunita Devi', '9222222222', NULL, '12 Karol Bagh, Delhi', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Prescriptions (includes columns added by migration 046: DoctorRegistrationNumber, PatientAge)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Prescriptions WHERE Id = '{PrescriptionId1}')
            INSERT INTO PMS.Prescriptions (Id, PatientId, PrescribingDoctor, PrescriptionDate, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{PrescriptionId1}', '{PatientId1}', 'Dr. A. Verma', SYSDATETIMEOFFSET(), 'Take after meals', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // PrescriptionItems
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.PrescriptionItems WHERE Id = '{PrescriptionItemId1}')
            INSERT INTO PMS.PrescriptionItems (Id, PrescriptionId, DrugId, Dosage, Quantity, Instructions, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{PrescriptionItemId1}', '{PrescriptionId1}', '{DrugId1}', '500mg twice daily', 14, 'Take with water', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DrugInventory
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DrugInventory WHERE Id = '{DrugInventoryId1}')
            INSERT INTO PMS.DrugInventory (Id, DrugId, BatchNumber, ExpirationDate, QuantityInStock, StorageConditions, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DrugInventoryId1}', '{DrugId1}', 'BATCH-AMX-001', '2026-12-31T00:00:00+05:30', 200, 'Store below 25°C', SYSDATETIMEOFFSET(), 'seed', 1);

            IF NOT EXISTS (SELECT 1 FROM PMS.DrugInventory WHERE Id = '{DrugInventoryId2}')
            INSERT INTO PMS.DrugInventory (Id, DrugId, BatchNumber, ExpirationDate, QuantityInStock, StorageConditions, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DrugInventoryId2}', '{DrugId2}', 'BATCH-PCT-001', '2027-06-30T00:00:00+05:30', 500, 'Store below 30°C', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DrugPricing (includes columns added by migration 043: Mrp, HsnCode)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DrugPricing WHERE Id = '{DrugPricingId1}')
            INSERT INTO PMS.DrugPricing (Id, DrugId, CostPrice, SellingPrice, Discount, GstRate, EffectiveFrom, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DrugPricingId1}', '{DrugId1}', 55.00, 75.00, 0.00, 12.00, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DrugInventoryRackAssignment
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DrugInventoryRackAssignment WHERE Id = '{DrugInventoryRackId1}')
            INSERT INTO PMS.DrugInventoryRackAssignment (Id, DrugInventoryId, RackId, QuantityPlaced, PlacedAt, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DrugInventoryRackId1}', '{DrugInventoryId1}', '{RackId1}', 200, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // ExpiryAlertConfiguration
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.ExpiryAlertConfiguration WHERE Id = '{ExpiryAlertConfigId1}')
            INSERT INTO PMS.ExpiryAlertConfiguration (Id, ThresholdDays, AlertType, IsEnabled, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{ExpiryAlertConfigId1}', 90, 'Email', 1, SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // ExpiryRecords
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.ExpiryRecords WHERE Id = '{ExpiryRecordId1}')
            INSERT INTO PMS.ExpiryRecords (Id, DrugInventoryId, DetectedAt, ExpirationDate, QuantityAffected, Status, InitiatedBy, QuarantineRackId, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{ExpiryRecordId1}', '{DrugInventoryId2}', SYSDATETIMEOFFSET(), '2025-01-31T00:00:00+05:30', 20, 'Quarantined', 'Priya Sharma', '{RackId2}', 'Batch expired during audit', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DisposalRecords
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DisposalRecords WHERE Id = '{DisposalRecordId1}')
            INSERT INTO PMS.DisposalRecords (Id, ExpiryRecordId, DisposedAt, QuantityDisposed, DisposalMethod, DisposedBy, WitnessedBy, RegulatoryReferenceNumber, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DisposalRecordId1}', '{ExpiryRecordId1}', SYSDATETIMEOFFSET(), 20, 'Incineration', 'Priya Sharma', 'Ramesh Kumar', 'REG/2026/001', 'Disposed as per CPCB norms', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // VendorExpiryReturnRequests
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.VendorExpiryReturnRequests WHERE Id = '{VendorExpiryReturnId1}')
            INSERT INTO PMS.VendorExpiryReturnRequests (Id, ExpiryRecordId, VendorId, RequestedAt, QuantityToReturn, Status, VendorNotes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{VendorExpiryReturnId1}', '{ExpiryRecordId1}', '{VendorId2}', SYSDATETIMEOFFSET(), 20, 'Pending', 'Return request for expired batch', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // QuotationRequests
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.QuotationRequests WHERE Id = '{QuotationRequestId1}')
            INSERT INTO PMS.QuotationRequests (Id, RequestDate, RequiredByDate, Status, Notes, RequestedBy, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{QuotationRequestId1}', SYSDATETIMEOFFSET(), DATEADD(day, 7, SYSDATETIMEOFFSET()), 'Pending', 'Monthly stock replenishment', 'admin', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // QuotationRequestItems
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.QuotationRequestItems WHERE Id = '{QuotationRequestItemId1}')
            INSERT INTO PMS.QuotationRequestItems (Id, QuotationRequestId, DrugId, QuantityRequired, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{QuotationRequestItemId1}', '{QuotationRequestId1}', '{DrugId1}', 500, 'Urgently required', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Quotations
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Quotations WHERE Id = '{QuotationId1}')
            INSERT INTO PMS.Quotations (Id, QuotationRequestId, VendorId, QuotationDate, ValidUntil, Status, TotalAmount, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{QuotationId1}', '{QuotationRequestId1}', '{VendorId1}', SYSDATETIMEOFFSET(), DATEADD(day, 30, SYSDATETIMEOFFSET()), 'Received', 27500.00, 'Best price offer', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // QuotationItems
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.QuotationItems WHERE Id = '{QuotationItemId1}')
            INSERT INTO PMS.QuotationItems (Id, QuotationId, DrugId, QuantityOffered, UnitPrice, TotalPrice, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{QuotationItemId1}', '{QuotationId1}', '{DrugId1}', 500, 55.00, 27500.00, NULL, SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // PurchaseOrders (includes columns: QuotationId from migration 031, PoNumber/ApprovedBy/etc from 049)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.PurchaseOrders WHERE Id = '{PurchaseOrderId1}')
            INSERT INTO PMS.PurchaseOrders (Id, VendorId, OrderDate, Status, Notes, TotalAmount, QuotationId, PoNumber, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{PurchaseOrderId1}', '{VendorId1}', SYSDATETIMEOFFSET(), 'Approved', 'Monthly stock order', 27500.00, '{QuotationId1}', 'PO/2026/001', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // PurchaseOrderItems
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.PurchaseOrderItems WHERE Id = '{PurchaseOrderItemId1}')
            INSERT INTO PMS.PurchaseOrderItems (Id, PurchaseOrderId, DrugId, QuantityOrdered, QuantityReceived, UnitPrice, BatchNumber, ExpirationDate, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{PurchaseOrderItemId1}', '{PurchaseOrderId1}', '{DrugId1}', 500, 500, 55.00, 'BATCH-AMX-001', '2026-12-31T00:00:00+05:30', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // PaymentLedger
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.PaymentLedger WHERE Id = '{PaymentLedgerId1}')
            INSERT INTO PMS.PaymentLedger (Id, VendorId, PurchaseOrderId, InvoicedAmount, PaidAmount, DueDate, Status, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{PaymentLedgerId1}', '{VendorId1}', '{PurchaseOrderId1}', 27500.00, 10000.00, DATEADD(day, 30, SYSDATETIMEOFFSET()), 'Partial', 'Partial payment made', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // CustomerInvoices (includes columns from migration 044: CgstAmount, SgstAmount, IgstAmount, InvoiceNumber)
        // and from migration 053: PrescriptionId
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.CustomerInvoices WHERE Id = '{CustomerInvoiceId1}')
            INSERT INTO PMS.CustomerInvoices (Id, PatientId, InvoiceDate, SubTotal, DiscountAmount, GstAmount, NetAmount, PaymentMethod, Status, Notes, PrescriptionId, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{CustomerInvoiceId1}', '{PatientId1}', SYSDATETIMEOFFSET(), 75.00, 0.00, 9.00, 84.00, 'Cash', 'Paid', NULL, '{PrescriptionId1}', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // CustomerInvoiceItems (includes columns from migration 045: CgstRate, SgstRate, IgstRate)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.CustomerInvoiceItems WHERE Id = '{CustomerInvoiceItemId1}')
            INSERT INTO PMS.CustomerInvoiceItems (Id, InvoiceId, DrugId, BatchNumber, Quantity, UnitPrice, DiscountPercent, GstRate, Amount, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{CustomerInvoiceItemId1}', '{CustomerInvoiceId1}', '{DrugId1}', 'BATCH-AMX-001', 1, 75.00, 0.00, 12.00, 84.00, SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // CustomerSubscriptions (includes ApprovalStatus from migration 050)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.CustomerSubscriptions WHERE Id = '{CustomerSubscriptionId1}')
            INSERT INTO PMS.CustomerSubscriptions (Id, PatientId, StartDate, EndDate, Status, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{CustomerSubscriptionId1}', '{PatientId1}', SYSDATETIMEOFFSET(), DATEADD(month, 3, SYSDATETIMEOFFSET()), 'Active', 'Monthly medication subscription', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // CustomerSubscriptionItems
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.CustomerSubscriptionItems WHERE Id = '{CustomerSubscriptionItemId1}')
            INSERT INTO PMS.CustomerSubscriptionItems (Id, CustomerSubscriptionId, DrugId, QuantityPerCycle, FrequencyDays, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{CustomerSubscriptionItemId1}', '{CustomerSubscriptionId1}', '{DrugId2}', 2, 30, 'One strip per month', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // SubscriptionFulfillments
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.SubscriptionFulfillments WHERE Id = '{SubscriptionFulfillmentId1}')
            INSERT INTO PMS.SubscriptionFulfillments (Id, CustomerSubscriptionId, FulfillmentDate, Status, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{SubscriptionFulfillmentId1}', '{CustomerSubscriptionId1}', SYSDATETIMEOFFSET(), 'Fulfilled', 'March cycle fulfilled', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DebtRecords
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DebtRecords WHERE Id = '{DebtRecordId1}')
            INSERT INTO PMS.DebtRecords (Id, PatientId, DebtDate, TotalAmount, RemainingAmount, Status, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DebtRecordId1}', '{PatientId2}', SYSDATETIMEOFFSET(), 500.00, 300.00, 'Open', 'Patient owes for Feb medicines', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DebtPayments
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DebtPayments WHERE Id = '{DebtPaymentId1}')
            INSERT INTO PMS.DebtPayments (Id, DebtRecordId, PaymentDate, AmountPaid, PaymentMethod, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DebtPaymentId1}', '{DebtRecordId1}', SYSDATETIMEOFFSET(), 200.00, 'Cash', 'Partial payment received', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DebtReminders
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DebtReminders WHERE Id = '{DebtReminderId1}')
            INSERT INTO PMS.DebtReminders (Id, DebtRecordId, ReminderDate, ReminderType, Message, IsSent, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DebtReminderId1}', '{DebtRecordId1}', SYSDATETIMEOFFSET(), 'SMS', 'Dear patient, please clear your outstanding balance of Rs 300.', 1, SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DamageRecords
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DamageRecords WHERE Id = '{DamageRecordId1}')
            INSERT INTO PMS.DamageRecords (Id, DrugId, RecordDate, QuantityDamaged, Reason, Status, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DamageRecordId1}', '{DrugId2}', SYSDATETIMEOFFSET(), 5, 'Dropped during shelving', 'Pending', 'Strips damaged during handling', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DamageDisposalRecords
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DamageDisposalRecords WHERE Id = '{DamageDisposalRecordId1}')
            INSERT INTO PMS.DamageDisposalRecords (Id, DamageRecordId, DisposalDate, DisposalMethod, DisposedBy, Notes, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DamageDisposalRecordId1}', '{DamageRecordId1}', SYSDATETIMEOFFSET(), 'Bin disposal', 'Priya Sharma', 'Damaged stock disposed', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // DailyDiaryEntries
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.DailyDiaryEntries WHERE Id = '{DailyDiaryEntryId1}')
            INSERT INTO PMS.DailyDiaryEntries (Id, EntryDate, Title, Content, Category, CreatedBy, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{DailyDiaryEntryId1}', SYSDATETIMEOFFSET(), 'Daily Operations Log', 'Received stock from Amit Pharmaceuticals. Dispensed 14 prescriptions. No incidents.', 'Operations', 'admin', SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // Notifications
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.Notifications WHERE Id = '{NotificationId1}')
            INSERT INTO PMS.Notifications (Id, Title, Message, NotificationType, RecipientId, IsRead, SentAt, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{NotificationId1}', 'Low Stock Alert', 'Amoxicillin 500mg is approaching reorder level.', 'StockAlert', '{UserId1}', 0, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 'seed', 1);
        ");

        // AuditLogs (DrugId, CustomerInvoiceId are NOT NULL; UpdatedAt/UpdatedBy are NOT NULL)
        Execute.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM PMS.AuditLogs WHERE Id = '{AuditLogId1}')
            INSERT INTO PMS.AuditLogs (Id, DrugId, DrugName, ScheduleCategory, CustomerInvoiceId, PrescriptionId, PatientId, QuantityDispensed, PerformedBy, PerformedAt, RetentionUntil, UpdatedAt, UpdatedBy, IsActive)
            VALUES ('{AuditLogId1}', '{DrugId1}', 'Amoxicillin 500mg', 'H1', '{CustomerInvoiceId1}', '{PrescriptionId1}', '{PatientId1}', 1, 'pharmacist1', SYSDATETIMEOFFSET(), DATEADD(year, 5, SYSDATETIMEOFFSET()), SYSDATETIMEOFFSET(), 'seed', 1);
        ");
    }

    public override void Down()
    {
        Execute.Sql($"DELETE FROM PMS.AuditLogs WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Notifications WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DailyDiaryEntries WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DamageDisposalRecords WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DamageRecords WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DebtReminders WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DebtPayments WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DebtRecords WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.SubscriptionFulfillments WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.CustomerSubscriptionItems WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.CustomerSubscriptions WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.CustomerInvoiceItems WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.CustomerInvoices WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.PaymentLedger WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.PurchaseOrderItems WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.PurchaseOrders WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.QuotationItems WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Quotations WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.QuotationRequestItems WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.QuotationRequests WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.VendorExpiryReturnRequests WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DisposalRecords WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.ExpiryRecords WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.ExpiryAlertConfiguration WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DrugInventoryRackAssignment WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DrugPricing WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DrugInventory WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.PrescriptionItems WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Prescriptions WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Patients WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Branches WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Racks WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.StorageZones WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.UserRoles WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Roles WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Users WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Vendors WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.Drugs WHERE UpdatedBy = 'seed';");
        Execute.Sql($"DELETE FROM PMS.DrugCategories WHERE UpdatedBy = 'seed';");
    }
}
