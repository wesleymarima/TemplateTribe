using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Account Sub-Category - Second level classification
/// </summary>
public class AccountSubCategory : BaseAuditableEntity
{
    [Required] [MaxLength(50)] public string Code { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    public int AccountCategoryId { get; set; }

    [ForeignKey(nameof(AccountCategoryId))]
    public AccountCategory AccountCategory { get; set; } = null!;

    public NormalBalance NormalBalance { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    // Navigation
    public ICollection<AccountType> AccountTypes { get; set; } = new List<AccountType>();
}
