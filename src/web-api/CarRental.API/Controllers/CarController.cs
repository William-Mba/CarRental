using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;

        public CarController(ICarRepository carRepository)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }

        /// <summary>
        /// Gets list with availables cars for rent
        /// </summary>
        /// <returns>
        /// List with available cars for rent
        /// </returns>
        /// <response code="200">List with cars</response>
        /// <response code="401">Access denied</response>
        /// <response code="404">Cars list not found</response>
        /// <response code="500">Opps! something went wrong</response>
        [ProducesResponseType(typeof(IReadOnlyList<Car>), 200)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCarsAsync()
        {
            var allcars = await _carRepository.GetAllAsync();

            return Ok(allcars);
        }

        /// <summary>
        /// Gets details about specific car
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Details about specific car
        /// </returns>
        /// <response code="200">Details about specific cars</response>
        /// <response code="401">Access denied</response>
        /// <response code="404">Cars list not found</response>
        /// <response code="500">Opps! something went wrong</response>
        public async Task<IActionResult> GetCarDetailsAsync(string id)
        {
            var carDetails = await _carRepository.GetAsync(id);

            return Ok(carDetails);
        }
    }
}
