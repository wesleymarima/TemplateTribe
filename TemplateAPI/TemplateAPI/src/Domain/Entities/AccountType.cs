using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Account Types - Third level classification
/// </summary>
public class AccountType : BaseAuditableEntity
{
    [Required] [MaxLength(50)] public string Code { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    public int AccountSubCategoryId { get; set; }

    [ForeignKey(nameof(AccountSubCategoryId))]
    public AccountSubCategory AccountSubCategory { get; set; } = null!;

    public NormalBalance NormalBalance { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    // Navigation
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
