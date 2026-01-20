namespace TemplateAPI.Domain.Entities;

public class Company : BaseAuditableEntity
{
    // Basic Information
    public string Name { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;

    // Address Information
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Financial Information
    public Currency Currency { get; set; } = null!;
    public int CurrencyId { get; set; }
    public int FiscalYearStartMonth { get; set; } = 1;


    // Status
    public bool IsActive { get; set; } = true;

    // Relationships
    public ICollection<Branch> Branches { get; set; } = new List<Branch>();
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<CostCenter> CostCenters { get; set; } = new List<CostCenter>();
    public ICollection<ExchangeRate> ExchangeRates { get; set; } = new List<ExchangeRate>();
    public ICollection<FinancialPeriod> FinancialPeriods { get; set; } = new List<FinancialPeriod>();
}
