namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Represents a currency in the system (ISO format)
///     Requirement: CUR-01, CUR-02
/// </summary>
public class Currency : BaseAuditableEntity
{
    /// <summary>
    ///     ISO Currency Code (e.g., USD, EUR, GBP)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Currency Name (e.g., US Dollar)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Currency Symbol (e.g., $, €, £)
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    ///     Number of decimal places (typically 2)
    /// </summary>
    public int DecimalPlaces { get; set; } = 2;

    /// <summary>
    ///     Indicates if currency is active for new transactions
    ///     Requirement: CUR-02
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Relationships
    public ICollection<ExchangeRate> ExchangeRates { get; set; } = new List<ExchangeRate>();
    public ICollection<Company> Companies { get; set; } = new List<Company>();
}
