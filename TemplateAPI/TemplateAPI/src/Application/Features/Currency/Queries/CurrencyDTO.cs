namespace TemplateAPI.Application.Features.Currency.Queries;

public class CurrencyDTO
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;

    public int DecimalPlaces { get; set; }

    public bool IsActive { get; set; }

    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Currency, CurrencyDTO>();
        }
    }
}
