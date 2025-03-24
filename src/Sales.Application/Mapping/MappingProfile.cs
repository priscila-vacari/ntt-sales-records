using AutoMapper;
using Sales.Application.DTOs;
using Sales.Domain.Entities;

namespace Sales.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<SaleDTO, Sale>().ReverseMap();
            CreateMap<SaleItemDTO, SaleItem>().ReverseMap();
        }
    }
}
