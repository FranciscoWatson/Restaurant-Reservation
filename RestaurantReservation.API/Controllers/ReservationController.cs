using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.DTOs.ReservationDto;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.ReservationRepository;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationController(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetCustomerById(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ReservationDto>(reservation));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDtoForCreation reservationDtoForCreation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var reservation = _mapper.Map<Reservation>(reservationDtoForCreation);

            await _reservationRepository.AddAsync(reservation);

            var reservationDto = _mapper.Map<ReservationDto>(reservation);
            
            return Ok(reservationDto);

        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReservationForUpdateDto reservationForUpdateDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            
            if (reservation == null)
            {
                return NotFound();
            }
            
            _mapper.Map(reservationForUpdateDto, reservation);
            
            await _reservationRepository.UpdateAsync(reservation);
            
            return NoContent();
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateReservation(int id, [FromBody] JsonPatchDocument<ReservationForUpdateDto> patchDocument)
        {
            var reservationEntity = await _reservationRepository.GetByIdAsync(id);

            if (reservationEntity is null)
            {
                return NotFound();
            }

            var reservationToPatch = _mapper.Map<ReservationForUpdateDto>(reservationEntity);

            patchDocument.ApplyTo(reservationToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(reservationToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(reservationToPatch, reservationEntity);

            await _reservationRepository.UpdateAsync(reservationEntity);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
                
            if (reservation == null)
            {
                return NotFound();
            }
            
            await _reservationRepository.DeleteAsync(id);

            return NoContent();
        }
        
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsByCustomerId(int customerId)
        {
            var reservations = await _reservationRepository.GetReservationsByCustomer(customerId);
            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
        }   
    }
}

