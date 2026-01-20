namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Represents a department within a company.
///     Requirements: DEPT-01 through DEPT-04
/// </summary>
public class Department : BaseAuditableEntity
{
    /// <summary>
    ///     Unique Department Code within company - Requirement: DEPT-01
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Department Name - Requirement: DEPT-01 (Mandatory)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Department Description - Requirement: DEPT-01 (Optional)
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///     Department Manager - Requirement: DEPT-01 (Optional)
    /// </summary>
    public string? ManagerId { get; set; }

    /// <summary>
    ///     Parent Department for hierarchical structure - Requirement: DEPT-02 (Optional)
    /// </summary>
    public int? ParentDepartmentId { get; set; }

    public Department? ParentDepartment { get; set; }

    /// <summary>
    ///     Child departments for hierarchical structure - Requirement: DEPT-02
    /// </summary>
    public ICollection<Department> ChildDepartments { get; set; } = new List<Department>();

    /// <summary>
    ///     Parent company relationship
    /// </summary>
    public int CompanyId { get; set; }

    public Company Company { get; set; } = null!;

    /// <summary>
    ///     Department activation status - Requirement: DEPT-04
    ///     Deactivated departments shall not be selectable for new transactions.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
