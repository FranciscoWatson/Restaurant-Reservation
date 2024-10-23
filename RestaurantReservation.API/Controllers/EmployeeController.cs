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

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var customers = await _employeeRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(customers));
        }

        /// <summary>
        /// Retrieves a specific employee by ID.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>Returns the employee data.</returns>
        /// <response code="200">Returned if the employee was found.</response>
        /// <response code="404">Returned if the employee is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> GetCustomerById(int id)
        {
            var customer = await _employeeRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EmployeeDto>(customer));
        }
        
        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employeeForCreationDto">The employee data to create.</param>
        /// <returns>A newly created employee.</returns>
        /// <response code="200">Returned if the employee is successfully created.</response>
        /// <response code="400">Returned if the request data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> CreateCustomer([FromBody] EmployeeForCreationDto employeeForCreationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var employee = _mapper.Map<Employee>(employeeForCreationDto);

            await _employeeRepository.AddAsync(employee);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            
            return Ok(employeeDto);
        }
        
        /// <summary>
        /// Updates an employee.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="employeeForUpdateDto">The updated employee data.</param>
        /// <response code="204">Returned if the employee is successfully updated.</response>
        /// <response code="404">Returned if the employee is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Partially updates an employee.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="patchDocument">The patch document for update.</param>
        /// <response code="204">Returned if the employee is successfully updated.</response>
        /// <response code="400">Returned if the request data is invalid.</response>
        /// <response code="404">Returned if the employee is not found.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes a specific employee.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <response code="204">Returned if the employee is successfully deleted.</response>
        /// <response code="404">Returned if the employee is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retrieves all managers.
        /// </summary>
        /// <returns>A list of all managers.</returns>
        [HttpGet("managers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetManagers()
        {
            var managers = await _employeeRepository.ListManagersAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(managers));
        }
        
        /// <summary>
        /// Retrieves the average order amount for a specific employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>The average order amount.</returns>
        [HttpGet("{employeeId}/average-order-amount")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetAverageOrderAmount(int employeeId)
        {
            var averageOrderAmount = await _employeeRepository.CalculateAverageOrderAmountAsync(employeeId);
            return Ok(averageOrderAmount);
        }
    }
}

