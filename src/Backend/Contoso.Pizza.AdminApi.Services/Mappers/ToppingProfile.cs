using AutoMapper;
using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.Data.Models;

namespace Contoso.Pizza.AdminApi.Services.Mappers;

public class ToppingProfile : Profile
{
    public ToppingProfile()
    {
        CreateMap<Topping, ToppingEntity>()
            .ReverseMap();
    }
}
