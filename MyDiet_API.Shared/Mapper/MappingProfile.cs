using AutoMapper;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.Shared.Models;

namespace MyDiet_API.Shared.Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>();

            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<ProductCategoryDto, ProductCategory>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<Meal, MealDto>();
            CreateMap<MealDto, Meal>();

            CreateMap<Diet, DietDto>();
            CreateMap<DietDto, Diet>();
        }
    }
}
