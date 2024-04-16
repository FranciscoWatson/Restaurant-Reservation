using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.DTOs.EmployeeDTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.CustomerRepository;
using RestaurantReservation.Db.Repositories.EmployeeRepository;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        
        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository; 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var customers = await _employeeRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(customers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetCustomerById(int id)
        {
            var customer = await _employeeRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EmployeeDto>(customer));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] EmployeeForCreationDto employeeForCreationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var employee = _mapper.Map<Employee>(employeeForCreationDto);

            await _employeeRepository.AddAsync(employee);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            
            return Ok(employeeDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            
            if (employee == null)
            {
                return NotFound();
            }
            
            _mapper.Map(employeeForUpdateDto, employee);
            
            await _employeeRepository.UpdateAsync(employee);
            
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateEmployee(int id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDocument)
        {
            var employeeEntity = await _employeeRepository.GetByIdAsync(id);

            if (employeeEntity is null)
            {
                return NotFound();
            }

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            patchDocument.ApplyTo(employeeToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(employeeToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(employeeToPatch, employeeEntity);

            await _employeeRepository.UpdateAsync(employeeEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
                
            if (employee == null)
            {
                return NotFound();
            }
            
            await _employeeRepository.DeleteAsync(id);

            return NoContent();
        }
        
        [HttpGet("managers")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetManagers()
        {
            var managers = await _employeeRepository.ListManagersAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(managers));
        }
        
        [HttpGet("{employeeId}/average-order-amount")]
        public async Task<ActionResult<decimal>> GetAverageOrderAmount(int employeeId)
        {
            var averageOrderAmount = await _employeeRepository.CalculateAverageOrderAmountAsync(employeeId);
            return Ok(averageOrderAmount);
        }
    }
}

