namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Represents exchange rates for currency conversions
///     Requirements: CUR-03, CUR-04
/// </summary>
public class ExchangeRate : BaseAuditableEntity
{
    /// <summary>
    ///     Source Currency
    /// </summary>
    public int CurrencyId { get; set; }

    public Currency Currency { get; set; } = null!;

    /// <summary>
    ///     Target Currency Code (typically base currency like USD)
    /// </summary>
    public string ToCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    ///     Exchange Rate Value - Requirement: CUR-03
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    ///     Effective Date - Requirement: CUR-03
    ///     Exchange rates shall be stored with effective date
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    ///     End Date (optional) - for rate validity period
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    ///     Indicates if this is the current active rate
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    ///     Company this rate applies to
    /// </summary>
    public int CompanyId { get; set; }

    public Company Company { get; set; } = null!;
}
