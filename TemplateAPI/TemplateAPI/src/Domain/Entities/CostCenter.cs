namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Represents a cost center for expense tracking and budgeting
///     Requirements: CC-01 through CC-05
/// </summary>
public class CostCenter : BaseAuditableEntity
{
    /// <summary>
    ///     Unique Cost Center Code within company - Requirement: CC-01 (Mandatory)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Cost Center Name - Requirement: CC-01 (Mandatory)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Cost Center Description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///     Parent Cost Center for hierarchical structure - Requirement: CC-02
    /// </summary>
    public int? ParentCostCenterId { get; set; }

    public CostCenter? ParentCostCenter { get; set; }

    /// <summary>
    ///     Cost center activation status - Requirement: CC-04
    ///     Inactive cost centers shall be excluded from transaction entry
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    ///     Indicates if cost center is mandatory for transactions - Requirement: CC-03
    /// </summary>
    public bool IsMandatoryForExpenses { get; set; } = true;

    public bool IsMandatoryForProcurement { get; set; } = true;
    public bool IsMandatoryForJournalEntries { get; set; } = false;

    // Parent Company
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    // Relationships - Requirement: CC-02
    public ICollection<CostCenter> ChildCostCenters { get; set; } = new List<CostCenter>();
}
