using AutoMapper;
using Contoso.Pizza.AdminApi.Models;
using DM = Contoso.Pizza.Data.Models;

namespace Contoso.Pizza.AdminApi.Services.Mappers;

public class PizzaProfile : Profile
{
    public PizzaProfile()
    {
        CreateMap<DM.Pizza, PizzaEntity>()
            .ReverseMap();
    }
}
