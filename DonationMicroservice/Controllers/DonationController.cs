using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using AutoMapper;

using DonationMicroservice.Models;
using DonationMicroservice.Models.DTO;
using DonationMicroservice.Repository;

namespace DonationMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IDonationRepository _donorRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<DonationController> _logger;
        public DonationController(IDonationRepository donorRepo, IMapper mapper, ILogger<DonationController> logger)
        {
            _donorRepo = donorRepo;
            _mapper = mapper;
            _logger = logger;
        }
    
        //--------------------------------------------------------------------------
        //General Operations
        //----Get All Donors----------------------------------------------------------------------
        [Authorize(Roles= "ADMIN, EMPLOYEE")]
        [HttpGet("GetAllDonors")]
        public async Task<IActionResult> GetAllDonors()
        {
            IEnumerable<Donor> donors = await _donorRepo.GetAllDonors();
            IEnumerable<DonorDTO> donorsDTO = _mapper.Map<List<DonorDTO>>(donors);
            return Ok(donorsDTO);
        }

        //----Get Donor----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetDonorById/{Id}")]
        public async Task<IActionResult> GetDonorById(int Id)
        {
            //Checking if Donor Exist
            DonorDTO donor = _mapper.Map<DonorDTO>(await _donorRepo.GetDonorById(Id));
          
            if (donor != null)
            {
                _logger.LogInformation("Output Donor Details");
                return Ok(donor);
            }
            _logger.LogWarning("Attempt to Get a Donor that donot exist in the database");
            return NotFound(new { message = "Donor not found" });
        }

        //--------------------------------------------------------------------------

        //==========================================================================



        //--------------------------------------------------------------------------
        //Authorised Donor Operations
        //----Add Donor----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPost("AddDonor")]
        public async Task<IActionResult> AddDonor([FromBody] DonorDTO donorDTO)
        {
            try
            {
                //----Exceptions

                //----*************
                //Adding Donor
                Donor donor = _mapper.Map<Donor>(donorDTO);
                var res = await _donorRepo.AddDonor(donor);
                if (res)
                {
                    _logger.LogInformation("Donor Added");
                    return Ok(new { message = "Donor Added Successfully" });
                }

                else
                {
                    _logger.LogWarning("Donor Adding Failure");
                    return StatusCode(500, new { message = "Donor Add Failure" });
                }
            }

            //----Catch ****
            catch (Exception ex) { return StatusCode(500, new { message = $"Donor Add Failure due to {ex.Message}" }); }

        }

        //----Update Donor----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPut("UpdateDonor/{Id}")]
        public async Task<IActionResult> UpdateDonor(int Id, [FromBody] DonorDTO donorDTO)
        {
            try 
            {
                //----Exceptions
                
                //----*************
                //Checking if Donor Id exist
                var existingDonor = await _donorRepo.GetDonorById(Id);
                if (existingDonor == null) return NotFound(new { message = "Donor not found" });

                //Updating Donor
                Donor donor = _mapper.Map<Donor>(donorDTO);
                var res = await _donorRepo.UpdateDonor(donor, Id);
                if (res) 
                {
                    _logger.LogInformation("Donor Updated");
                    return Ok(new { message = "Donor Updated Successfully" });
                }

                _logger.LogWarning("Donor Updating Failure: Donor Id doesnot exist");
                return StatusCode(500, new { message = "Donor Update Failure: Donor Id doesnot exist" });
            }

            //----Catch ****
            catch (Exception ex) { return StatusCode(500, new { message = $"Donor Add Failure due to {ex.Message}" }); }

        }

        //----Delete Donor----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpDelete("DeleteDonor/{Id}")]
        public async Task<IActionResult> DeleteDonor(int Id)
        {
            //Checking if Donor Exist in DataBase
            var donor = await _donorRepo.GetDonorById(Id);
            if (donor == null) { return BadRequest(new { message = "Donor Updating Failure: Donor doesnot exist" }); }

            
            //Deleting Donor
            var res = await _donorRepo.DeleteDonor(Id);
            if (res)
            {
                _logger.LogInformation("Donor Deleted");
                return Ok(new { message = "Donor Deleted Successfully" });
            }

            _logger.LogWarning("Donor Deleting Failure");
            return StatusCode(500, new { message = "Donor Delete Failure" });
            
        }

        //--------------------------------------------------------------------------

        //==========================================================================

    }
}
