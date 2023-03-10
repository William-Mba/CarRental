using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces.Message;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [Authorize(Policy = "AccessAsUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class CarReservationController : ControllerBase
    {

        private readonly ICarReservationService _carReservationService;
        private readonly ICarReservationMessagingService _carReservationMessagingService;
        private readonly IMapper _mapper;

        public CarReservationController(ICarReservationService carReservationService,
            ICarReservationMessagingService carReservationMessagingService, IMapper mapper)
        {
            _carReservationService = carReservationService ??
                throw new ArgumentException(nameof(carReservationService));

            _carReservationMessagingService = carReservationMessagingService ??
                throw new ArgumentException(nameof(carReservationMessagingService));

            _mapper = mapper ??
                throw new ArgumentException(nameof(mapper));
        }

        /// <summary>
        /// Make reservation for a specific car
        /// </summary>
        /// <param name="customerCarReservation"></param>
        /// <response code="201">Created customer reservation for a specific car</response>
        /// <response code="401">Access denied</response>
        /// <response code="400">Model is not valid or car is already reserved</response>
        /// <response code="500">Opps! something went wrong</response>
        /// <exception cref="NullReferenceException"></exception>
        [ProducesResponseType(201)]
        [HttpPost()]
        public async Task<IActionResult> CreateReservationAsync([FromBody] CarReservationDto customerCarReservation)
        {
            var carReservation = _mapper.Map<CarReservation>(customerCarReservation);

            var command = await _carReservationService.MakeReservationAsync(carReservation);

            if (command.CompletedWithSuccess)
            {
                if (command.Result == null)
                {
                    throw new NullReferenceException(nameof(command.Result));
                }
                await _carReservationMessagingService.PublishNewCarReservationMessageAsync(command.Result);

                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return BadRequest(command.OperationError);
            }
        }
    }
}
