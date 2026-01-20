namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Represents a branch/subsidiary of a parent company
///     Requirements: BR-01 through BR-05
/// </summary>
public class Branch : BaseAuditableEntity
{
    /// <summary>
    ///     Branch Name - Requirement: BR-01
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Unique Branch Code within company - Requirement: BR-01, BR-02
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Country - Requirement: BR-01 (Mandatory)
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    ///     City - Requirement: BR-01 (Mandatory)
    /// </summary>
    public string City { get; set; } = string.Empty;

    // Optional contact details - Requirement: BR-01
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Optional address information - Requirement: BR-01
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    // Branch Type and Configuration
    public string BranchType { get; set; } = "Office"; // Office, Warehouse, Store, Factory, etc.
    public bool IsHeadquarters { get; set; } = false;
    public string BusinessHours { get; set; } = string.Empty;

    /// <summary>
    ///     Branch activation status - Requirement: BR-03, BR-04
    ///     Inactive branches shall not be available for new transactions
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Branch Manager - Requirement: BR-05
    public int? ManagerId { get; set; }

    // Parent Company - Requirement: BR-01
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    // Relationships - Requirement: BR-05
    public ICollection<Person> Persons { get; set; } = new List<Person>();
}
