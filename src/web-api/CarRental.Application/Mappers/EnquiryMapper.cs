using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappers
{
    public class EnquiryMapper : Profile
    {
        public EnquiryMapper()
        {
            CreateMap<Enquiry, CustomerEnquiryDto>();

            CreateMap<CustomerEnquiryDto, Enquiry>();
        }
    }
}
