using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Core.Entities;
using Core.Extensions;
using Core.Services;
using System;

namespace Api.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IPasswordService passwordService)
        {
            string indianTimezoneId = "India Standard Time";

            #region Request mapping
            CreateMap<RegisterUserDto, User>()
                .ForMember(dto => dto.FirstName, options => options.MapFrom(src => src.FirstName))
                .ForMember(dto => dto.LastName, options => options.MapFrom(src => src.LastName))
                .ForMember(dto => dto.GenderAbbreviation, options => options.MapFrom(src => src.GenderAbbreviation))
                .ForMember(dto => dto.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToUniversalTime()))
                .ForMember(dto => dto.Phone, options => options.MapFrom(src => src.Phone))
                .ForMember(dto => dto.Email, options => options.MapFrom(src => src.Email))
                .ForMember(dto => dto.Password, options => options.MapFrom(src => passwordService.HashPassword(src.Password)))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(_ => DateTime.UtcNow));
            #endregion

            #region Response mapping
            CreateMap<User, UserResponseDto>()
                .ForMember(dto => dto.FirstName, options => options.MapFrom(src => src.FirstName))
                .ForMember(dto => dto.LastName, options => options.MapFrom(src => src.LastName))
                .ForMember(dto => dto.GenderAbbreviation, options => options.MapFrom(src => src.GenderAbbreviation))
                .ForMember(dto => dto.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ConvertToLocalFromUTC(indianTimezoneId)))
                .ForMember(dto => dto.Phone, options => options.MapFrom(src => src.Phone))
                .ForMember(dto => dto.Username, options => options.MapFrom(src => src.Username))
                .ForMember(dto => dto.Email, options => options.MapFrom(src => src.Email))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(src => src.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId)))
                .ForMember(dto => dto.UpdatedAt, options => options.MapFrom(src => src.UpdatedAt.HasValue
                        ? src.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : default(DateTime?)));
            #endregion
        }
    }
}
