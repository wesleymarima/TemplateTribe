using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Department.Commands.Delete;

public record DeleteDepartmentCommand(int Id) : IRequest<OperationResult>;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public DeleteDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Department? entity = await _context.Departments
            .Include(d => d.ChildDepartments)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Department), request.Id.ToString());
        }

        // Check if department has child departments
        if (entity.ChildDepartments.Any())
        {
            throw new ValidationException(
                "Cannot delete department with child departments. Please reassign or delete child departments first.");
        }

        // In a real system, check if department is referenced in transactions/workflows
        // For now, we'll allow deletion if no children

        string departmentName = entity.Name;
        string departmentCode = entity.Code;

        _context.Departments.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult(
            $"Department '{departmentName}' (Code: {departmentCode}) has been deleted successfully.");
    }
}
