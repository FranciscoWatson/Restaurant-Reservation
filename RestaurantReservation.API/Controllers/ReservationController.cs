using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.MenuItemDTOs;
using RestaurantReservation.API.DTOs.OrderDTOs;
using RestaurantReservation.API.DTOs.ReservationDto;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.ReservationRepository;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationController(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all reservations.
        /// </summary>
        /// <returns>A list of all reservations.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
        }
        
        /// <summary>
        /// Retrieves a specific reservation by ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>Returns the reservation data.</returns>
        /// <response code="200">Returned if the reservation was found.</response>
        /// <response code="404">Returned if the reservation is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservationDto>> GetCustomerById(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ReservationDto>(reservation));
        }
        
        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="reservationDtoForCreation">The reservation data to create.</param>
        /// <returns>A newly created reservation.</returns>
        /// <response code="200">Returned if the reservation is successfully created.</response>
        /// <response code="400">Returned if the request data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDtoForCreation reservationDtoForCreation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var reservation = _mapper.Map<Reservation>(reservationDtoForCreation);

            await _reservationRepository.AddAsync(reservation);

            var reservationDto = _mapper.Map<ReservationDto>(reservation);
            
            return Ok(reservationDto);

        }
        
        /// <summary>
        /// Updates a reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="reservationForUpdateDto">The updated reservation data.</param>
        /// <response code="204">Returned if the reservation is successfully updated.</response>
        /// <response code="404">Returned if the reservation is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        
        /// <summary>
        /// Partially updates a reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="patchDocument">The patch document for update.</param>
        /// <response code="204">Returned if the reservation is successfully updated.</response>
        /// <response code="400">Returned if the request data is invalid.</response>
        /// <response code="404">Returned if the reservation is not found.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        
        /// <summary>
        /// Deletes a specific reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <response code="204">Returned if the reservation is successfully deleted.</response>
        /// <response code="404">Returned if the reservation is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        
        /// <summary>
        /// Retrieves all reservations for a specific customer by customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose reservations are to be retrieved.</param>
        /// <returns>A list of reservations for the specified customer.</returns>
        /// <response code="200">Returns a list of reservations.</response>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsByCustomerId(int customerId)
        {
            var reservations = await _reservationRepository.GetReservationsByCustomer(customerId);
            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
        }  
        
        /// <summary>
        /// Retrieves all orders and their associated menu items for a specific reservation by reservation ID.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation whose orders are to be retrieved.</param>
        /// <returns>A list of orders and their menu items for the specified reservation.</returns>
        /// <response code="200">Returns a list of orders with their menu items.</response>
        [HttpGet("{reservationId}/orders")]
        [ProducesResponseType(typeof(IEnumerable<OrderWithMenuItemsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByReservationId(int reservationId)
        {
            var ordersWithMenuItems = await _reservationRepository.ListOrdersAndMenuItems(reservationId);
            return Ok(_mapper.Map<IEnumerable<OrderWithMenuItemsDto>>(ordersWithMenuItems));
        }
        
        /// <summary>
        /// Retrieves all menu items ordered in a specific reservation by reservation ID.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation whose menu items are to be retrieved.</param>
        /// <returns>A list of menu items ordered in the specified reservation.</returns>
        /// <response code="200">Returns a list of menu items.</response>
        [HttpGet("{reservationId}/menu-items")]
        [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItemsByReservationId(int reservationId)
        {
            var menuItems = await _reservationRepository.ListOrderedMenuItems(reservationId);
            return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(menuItems));
        }
    }
}

