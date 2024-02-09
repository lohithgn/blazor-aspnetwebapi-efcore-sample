using AutoMapper;
using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.Data.Models;

namespace Contoso.Pizza.AdminApi.Services.Mappers
{
    public class SauceProfile : Profile
    {
        public SauceProfile()
        {
            CreateMap<Sauce, SauceEntity>()
                .ReverseMap();
        }
    }
}
