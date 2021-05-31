using Api.Models.Requests;
using AutoMapper;
using Core.Entities;
using Core.Services;
using System;

namespace Api.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IPasswordService passwordService)
        {
            // Users mapping
            CreateMap<RegisterUserDto, User>()
                .ForMember(dto => dto.FirstName, options => options.MapFrom(src => src.FirstName))
                .ForMember(dto => dto.LastName, options => options.MapFrom(src => src.LastName))
                .ForMember(dto => dto.GenderAbbreviation, options => options.MapFrom(src => src.GenderAbbreviation))
                .ForMember(dto => dto.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToUniversalTime()))
                .ForMember(dto => dto.Phone, options => options.MapFrom(src => src.Phone))
                .ForMember(dto => dto.Email, options => options.MapFrom(src => src.Email))
                .ForMember(dto => dto.Password, options => options.MapFrom(src => passwordService.HashPassword(src.Password)))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
