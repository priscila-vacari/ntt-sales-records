using AutoMapper;
using Sales.API.Models;
using Sales.Application.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace Sales.API.Mapping
{
    /// <summary>
    /// Mapeamento de modelos
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MappingProfile: Profile
    {
        /// <summary>
        /// Perfis de mapeamento
        /// </summary>
        public MappingProfile()
        {
            CreateMap<SaleRequestModel, SaleDTO>().ReverseMap();
            CreateMap<SaleResponseModel, SaleDTO>().ReverseMap();
            CreateMap<SaleCreateResponseModel, SaleDTO>().ReverseMap();
            CreateMap<SaleItemRequestModel, SaleItemDTO>().ReverseMap();
            CreateMap<SaleItemResponseModel, SaleItemDTO>().ReverseMap();
        }
    }
}
