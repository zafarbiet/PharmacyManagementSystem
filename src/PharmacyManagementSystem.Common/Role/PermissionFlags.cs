namespace PharmacyManagementSystem.Common.Role;

[Flags]
public enum PermissionFlags : long
{
    None             = 0,
    InvoiceRead      = 1L << 0,
    InvoiceCreate    = 1L << 1,
    InvoiceVoid      = 1L << 2,
    PoRead           = 1L << 3,
    PoCreate         = 1L << 4,
    PoApprove        = 1L << 5,
    InventoryRead    = 1L << 6,
    InventoryAdjust  = 1L << 7,
    DrugRead         = 1L << 8,
    DrugManage       = 1L << 9,
    PatientRead      = 1L << 10,
    PatientManage    = 1L << 11,
    ReportsView      = 1L << 12,
    ReportsExport    = 1L << 13,
    VendorRead       = 1L << 14,
    VendorManage     = 1L << 15,
    UsersManage      = 1L << 16,
    RolesManage      = 1L << 17,
    SettingsManage   = 1L << 18,
    QuotationsManage = 1L << 19,
    DebtManage       = 1L << 20,
    AuditLogView     = 1L << 21,
}
