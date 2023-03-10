using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappers
{
    public class CarReservationMapper : Profile
    {
        public CarReservationMapper()
        {
            CreateMap<CarReservation, CarReservationDto>();

            CreateMap<CarReservationDto, CarReservation>();
        }
    }
}
