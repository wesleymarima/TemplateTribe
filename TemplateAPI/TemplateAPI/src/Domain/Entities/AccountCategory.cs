using System.ComponentModel.DataAnnotations;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Account Category - Top level classification
/// </summary>
public class AccountCategory : BaseAuditableEntity
{
    [Required] [MaxLength(50)] public string Code { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    public CategoryType Type { get; set; }

    public NormalBalance NormalBalance { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    // Navigation
    public ICollection<AccountSubCategory> SubCategories { get; set; } = new List<AccountSubCategory>();
}
