using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using AutoMapper;

using PersonMicroservice.Models;
using PersonMicroservice.Models.DTO;
using PersonMicroservice.Repository;

namespace PersonMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiverController : ControllerBase
    {
        private readonly IReceiverRepository _receiverRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ReceiverController> _logger;
        public ReceiverController(IReceiverRepository receiverRepo, IMapper mapper, ILogger<ReceiverController> logger)
        {
            _receiverRepo = receiverRepo;
            _mapper = mapper;
            _logger = logger;
        }
    
        //--------------------------------------------------------------------------
        //General Operations
        //----Get All Receivers----------------------------------------------------------------------
        [Authorize(Roles= "ADMIN, EMPLOYEE")]
        [HttpGet("GetAllReceivers")]
        public async Task<IActionResult> GetAllReceivers()
        {
            IEnumerable<Receiver> receivers = await _receiverRepo.GetAllReceivers();
            IEnumerable<ReceiverDTO> receiversDTO = _mapper.Map<List<ReceiverDTO>>(receivers);
            return Ok(receiversDTO);
        }

        //----Get Receiver----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetReceiverById/{Id}")]
        public async Task<IActionResult> GetReceiverById(int Id)
        {
            //Checking if Receiver Exist
            ReceiverDTO receiver = _mapper.Map<ReceiverDTO>(await _receiverRepo.GetReceiverById(Id));
          
            if (receiver != null)
            {
                _logger.LogInformation("Output Receiver Details");
                return Ok(receiver);
            }
            _logger.LogWarning("Attempt to Get a Receiver that donot exist in the database");
            return NotFound(new { message = "Receiver not found" });
        }

        //--------------------------------------------------------------------------

        //==========================================================================



        //--------------------------------------------------------------------------
        //Authorised Receiver Operations
        //----Add Receiver----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPost("AddReceiver")]
        public async Task<IActionResult> AddReceiver([FromBody] ReceiverDTO receiverDTO)
        {
            try
            {
                //----Exceptions

                //----*************
                //Adding Receiver
                Receiver receiver = _mapper.Map<Receiver>(receiverDTO);
                var res = await _receiverRepo.AddReceiver(receiver);
                if (res)
                {

                    _logger.LogInformation("Receiver Added");
                    return Ok(new { message = "Receiver Added Successfully" });
                }

                else
                {
                    _logger.LogWarning("Receiver Adding Failure");
                    return StatusCode(400, new { message = "Receiver Add Failure" });
                }
            }

            //----Catch ****
            catch (Exception ex) { return StatusCode(400, new { message = $"Receiver Add Failure due to {ex.Message}" }); }

        }

        //----Update Receiver----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPut("UpdateReceiver/{Id}")]
        public async Task<IActionResult> UpdateReceiver(int Id, [FromBody] ReceiverDTO receiverDTO)
        {
            try 
            {
                //----Exceptions
                
                //----*************
                //Checking if Receiver Id exist
                var existingReceiver = await _receiverRepo.GetReceiverById(Id);
                if (existingReceiver == null) return NotFound(new { message = "Receiver not found" });

                //Updating Receiver
                Receiver receiver = _mapper.Map<Receiver>(receiverDTO);
                var res = await _receiverRepo.UpdateReceiver(receiver, Id);
                if (res) 
                {
                    _logger.LogInformation("Receiver Updated");
                    return Ok(new { message = "Receiver Updated Successfully" });
                }

                _logger.LogWarning("Receiver Updating Failure: Receiver Id doesnot exist");
                return StatusCode(400, new { message = "Receiver Update Failure: Receiver Id doesnot exist" });
            }

            //----Catch ****
            catch (Exception ex) { return StatusCode(400, new { message = $"Receiver Add Failure due to {ex.Message}" }); }

        }

        //----Delete Receiver----------------------------------------------------------------------
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpDelete("DeleteReceiver/{Id}")]
        public async Task<IActionResult> DeleteReceiver(int Id)
        {
            //Checking if Receiver Exist in DataBase
            var receiver = await _receiverRepo.GetReceiverById(Id);
            if (receiver == null) { return BadRequest(new { message = "Receiver Updating Failure: Receiver doesnot exist" }); }


            //Deleting Receiver
            var res = await _receiverRepo.DeleteReceiver(Id);
            if (res)
            {
                _logger.LogInformation("Receiver Deleted");
                return Ok(new { message = "Receiver Deleted Successfully" });
            }

            _logger.LogWarning("Receiver Deleting Failure");
            return StatusCode(400, new { message = "Receiver Delete Failure" });
            
        }

        //--------------------------------------------------------------------------

        //==========================================================================

    }
}
