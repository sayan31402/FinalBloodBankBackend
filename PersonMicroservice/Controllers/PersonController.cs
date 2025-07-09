using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using AutoMapper;

using PersonMicroservice.Models;
using PersonMicroservice.Models.DTO;
using PersonMicroservice.Repository;
using Microsoft.AspNetCore.Cors;

namespace PersonMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonController> _logger;
        public PersonController(IPersonRepository personRepo, IMapper mapper, ILogger<PersonController> logger)
        {
            _personRepo = personRepo;
            _mapper = mapper;
            _logger = logger;
        }
    
        //--------------------------------------------------------------------------
        //General Operations
        //----Get All Persons----------------------------------------------------------------------
        [Authorize(Roles= "ADMIN, EMPLOYEE")]
        [HttpGet("GetAllPersons")]
        public async Task<IActionResult> GetAllPersons()
        {
            IEnumerable<Person> persons = await _personRepo.GetAllPersons();
            IEnumerable<PersonDTO> personsDTO = _mapper.Map<List<PersonDTO>>(persons);
            return Ok(personsDTO);
        }

        //----Get Person By Id----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetPersonById/{Id}")]
        public async Task<IActionResult> GetPersonById(int Id)
        {
            //Checking if Person Exist
            PersonDTO person = _mapper.Map<PersonDTO>(await _personRepo.GetPersonById(Id));
          
            if (person != null)
            {
                _logger.LogInformation("Output Person Details");
                return Ok(person);
            }
            _logger.LogWarning("Attempt to Get a Person that donot exist in the database");
            return NotFound(new { message = "Person not found" });
        }

        //----Get Person By Name----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetPersonByName/{Name}")]
        public async Task<IActionResult> GetPersonByName(string Name)
        {
            //Checking if Person Exist
            List<PersonDTO> persons = _mapper.Map<List<PersonDTO>>(await _personRepo.GetPersonByName(Name));

            if (persons != null)
            {
                _logger.LogInformation("Output Person Details");
                return Ok(persons);
            }
            _logger.LogWarning("Attempt to Get a Person that donot exist in the database");
            return NotFound(new { message = "Person not found" });
        }

        //----Get Person By BloodGroup----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetPersonByBloodGroup/{BloodGroup}")]
        public async Task<IActionResult> GetPersonByBloodGroup(string BloodGroup)
        {
            //Checking if Person Exist
            List<PersonDTO> persons = _mapper.Map<List<PersonDTO>>(await _personRepo.GetPersonByBloodGroup(BloodGroup));

            if (persons != null)
            {
                _logger.LogInformation("Output Person Details");
                return Ok(persons);
            }
            _logger.LogWarning("Attempt to Get a Person that donot exist in the database");
            return NotFound(new { message = "Person not found" });
        }

        //--------------------------------------------------------------------------

        //==========================================================================



        //--------------------------------------------------------------------------
        //Authorised Person Operations
        //----Add Person----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPost("AddPerson")]
        public async Task<IActionResult> AddPerson([FromBody] PersonDTO personDTO)
        {
            try
            {
                //----Exceptions

                //----*************
                //Adding Person
                Person person = _mapper.Map<Person>(personDTO);
                var res = await _personRepo.AddPerson(person);
                if (res)
                {
                    _logger.LogInformation("Person Added");
                    return Ok(new { message = "Person Added Successfully" });
                }

                else
                {
                    _logger.LogWarning("Person Adding Failure");
                    return StatusCode(400, new { message = "Person Add Failure" });
                }
            }

            //----Catch ****
            catch (Exception ex) { return StatusCode(400, new { message = $"Person Add Failure due to {ex.Message}" }); }

        }

        //----Update Person----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPut("UpdatePerson/{Id}")]
        public async Task<IActionResult> UpdatePerson(int Id, [FromBody] PersonDTO personDTO)
        {
            try 
            {
                //----Exceptions
                
                //----*************
                //Checking if Person Id exist
                var existingPerson = await _personRepo.GetPersonById(Id);
                if (existingPerson == null) return NotFound(new { message = "Person not found" });

                //Updating Person
                Person person = _mapper.Map<Person>(personDTO);
                var res = await _personRepo.UpdatePerson(person, Id);
                if (res) 
                {
                    _logger.LogInformation("Person Updated");
                    return Ok(new { message = "Person Updated Successfully" });
                }

                _logger.LogWarning("Person Updating Failure: Person Id doesnot exist");
                return StatusCode(400, new { message = "Person Update Failure: Person Id doesnot exist" });
            }

            //----Catch ****
            catch (Exception ex) { return StatusCode(400, new { message = $"Person Add Failure due to {ex.Message}" }); }

        }

        //----Delete Person----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpDelete("DeletePerson/{Id}")]
        public async Task<IActionResult> DeletePerson(int Id)
        {
            //Checking if Person Exist in DataBase
            var person = await _personRepo.GetPersonById(Id);
            if (person == null) { return BadRequest(new { message = "Person Updating Failure: Person doesnot exist" }); }

            
            //Deleting Person
            var res = await _personRepo.DeletePerson(Id);
            if (res)
            {
                _logger.LogInformation("Person Deleted");
                return Ok(new { message = "Person Deleted Successfully" });
            }

            _logger.LogWarning("Person Deleting Failure");
            return StatusCode(400, new { message = "Person Delete Failure" });
            
        }

        //--------------------------------------------------------------------------

        //==========================================================================

    }
}
