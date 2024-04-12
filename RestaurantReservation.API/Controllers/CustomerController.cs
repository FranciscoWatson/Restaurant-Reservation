using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.Db.Migrations;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.CustomerRepository;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;



namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        
        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CustomerDto>(customer));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDtoForCreation customerDtoForCreation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var customer = _mapper.Map<Customer>(customerDtoForCreation);

            await _customerRepository.AddAsync(customer);

            var customerDto = _mapper.Map<CustomerDto>(customer);
            
            return Ok(customerDto);

        }
        
        [HttpPut("{id}")]
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

        [HttpPatch("{id}")]
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

        [HttpDelete("{id}")]
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
