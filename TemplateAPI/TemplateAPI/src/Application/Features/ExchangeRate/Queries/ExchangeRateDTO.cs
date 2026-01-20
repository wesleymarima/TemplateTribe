namespace TemplateAPI.Application.Features.ExchangeRate.Queries;

public class ExchangeRateDTO
{
    public int Id { get; set; }

    public int CurrencyId { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string CurrencyName { get; set; } = string.Empty;

    public string ToCurrencyCode { get; set; } = string.Empty;

    public decimal Rate { get; set; }

    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;

    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.ExchangeRate, ExchangeRateDTO>()
                .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency.Code))
                .ForMember(d => d.CurrencyName, opt => opt.MapFrom(s => s.Currency.Name))
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.Name));
        }
    }
}
