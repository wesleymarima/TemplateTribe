namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Represents a financial period for transaction control
///     Requirements: FP-01 through FP-05
/// </summary>
public class FinancialPeriod : BaseAuditableEntity
{
    /// <summary>
    ///     Period Name (e.g., "January 2026", "Q1 2026")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Period Number within the financial year - Requirement: FP-01
    /// </summary>
    public int PeriodNumber { get; set; }

    /// <summary>
    ///     Financial Year - Requirement: FP-01
    /// </summary>
    public int FiscalYear { get; set; }

    /// <summary>
    ///     Period Start Date - Requirement: FP-01 (Mandatory)
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    ///     Period End Date - Requirement: FP-01 (Mandatory)
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    ///     Period Status - Requirement: FP-02
    /// </summary>
    public PeriodStatus Status { get; set; } = PeriodStatus.Open;

    /// <summary>
    ///     User who closed the period - Requirement: FP-04
    /// </summary>
    public string? ClosedBy { get; set; }

    /// <summary>
    ///     Timestamp when period was closed - Requirement: FP-04
    /// </summary>
    public DateTime? ClosedDate { get; set; }

    /// <summary>
    ///     User who reopened the period - Requirement: FP-05
    /// </summary>
    public string? ReopenedBy { get; set; }

    /// <summary>
    ///     Timestamp when period was reopened - Requirement: FP-05
    /// </summary>
    public DateTime? ReopenedDate { get; set; }

    /// <summary>
    ///     Reason for reopening (for audit) - Requirement: FP-05
    /// </summary>
    public string? ReopenReason { get; set; }

    // Parent Company
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}

/// <summary>
///     Financial Period Status enumeration - Requirement: FP-02
/// </summary>
public enum PeriodStatus
{
    Open = 1,
    Closed = 2
}
