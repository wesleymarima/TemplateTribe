namespace TemplateAPI.Domain.Entities;

public class Person : BaseAuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;

    public string ApplicationUserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;


    // Employment Information

    public bool IsActive { get; set; } = true;

    // Branch Assignment
    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
}
