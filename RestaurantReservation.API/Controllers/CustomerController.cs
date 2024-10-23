using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.CustomerRepository;



namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        
        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        
        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>A list of all customers.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }

        
        /// <summary>
        /// Retrieves a specific customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>Returns the customer data.</returns>
        /// <response code="200">Returned if the customer was found.</response>
        /// <response code="404">Returned if the customer is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CustomerDto>(customer));
        }
        
        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customerDtoForCreation">The customer data to create.</param>
        /// <returns>A newly created customer.</returns>
        /// <response code="200">Returned if the customer is successfully created.</response>
        /// <response code="400">Returned if the request data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDtoForCreation customerDtoForCreation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var customer = _mapper.Map<Customer>(customerDtoForCreation);

            await _customerRepository.AddAsync(customer);

            var customerDto = _mapper.Map<CustomerDto>(customer);
            
            return Ok(customerDto);

        }
        
        /// <summary>
        /// Updates a customer.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customerForUpdateDto">The updated customer data.</param>
        /// <response code="204">Returned if the customer is successfully updated.</response>
        /// <response code="404">Returned if the customer is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerForUpdateDto customerForUpdateDto)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            
            if (customer == null)
            {
                return NotFound();
            }
            
            _mapper.Map(customerForUpdateDto, customer);
            
            await _customerRepository.UpdateAsync(customer);
            
            return NoContent();
        }

        /// <summary>
        /// Partially updates a customer.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="patchDocument">The patch document for update.</param>
        /// <response code="204">Returned if the customer is successfully updated.</response>
        /// <response code="400">Returned if the request data is invalid.</response>
        /// <response code="404">Returned if the customer is not found.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartiallyUpdateCustomer(int id, [FromBody] JsonPatchDocument<CustomerForUpdateDto> patchDocument)
        {
            var customerEntity = await _customerRepository.GetByIdAsync(id);

            if (customerEntity is null)
            {
                return NotFound();
            }

            var customerToPatch = _mapper.Map<CustomerForUpdateDto>(customerEntity);

            patchDocument.ApplyTo(customerToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(customerToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(customerToPatch, customerEntity);

            await _customerRepository.UpdateAsync(customerEntity);

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific customer.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <response code="204">Returned if the customer is successfully deleted.</response>
        /// <response code="404">Returned if the customer is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
                
            if (customer == null)
            {
                return NotFound();
            }
            
            await _customerRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
