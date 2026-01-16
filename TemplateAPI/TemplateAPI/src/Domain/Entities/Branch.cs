namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Represents a branch/subsidiary of a parent company
/// </summary>
public class Branch : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Address Information
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    // Branch Type and Configuration
    public string BranchType { get; set; } = "Office"; // Office, Warehouse, Store, Factory, etc.
    public bool IsHeadquarters { get; set; } = false;
    public bool IsActive { get; set; } = true;


    // Parent Company
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    // Relationships
    public ICollection<Person> Persons { get; set; } = new List<Person>();
}
