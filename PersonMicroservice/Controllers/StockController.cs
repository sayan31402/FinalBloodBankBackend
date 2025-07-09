using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonMicroservice.Models.DTO;
using PersonMicroservice.Models;
using PersonMicroservice.Repository;
using Microsoft.AspNetCore.Authorization;

namespace PersonMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepo _stockRepo;
        private readonly IMapper _mapper;

        public StockController(IStockRepo stockRepo, IMapper mapper)
        {
            _stockRepo = stockRepo;
            _mapper = mapper;
        }

        //--------------------------------------------------------------------------
        // GET: api/Stock
        //[Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetAllStocks")]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _stockRepo.GetAllStocks();
            var stockDtos = _mapper.Map<IEnumerable<StockGetDTO>>(stocks);
            return Ok(stockDtos);
        }

        // GET: api/Stock/{bloodGroup}
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetStockByBloodGroup/{bloodGroup}")]
        public async Task<IActionResult> GetStockByBloodGroup(string bloodGroup)
        {
            var stock = await _stockRepo.GetStockByBloodGroup(bloodGroup);
            if (stock == null)
                return NotFound(new { message = "Stock Not Found" });

            var stockDto = _mapper.Map<StockGetDTO>(stock);
            return Ok(stockDto);
        }

        //--------------------------------------------------------------------------
        // POST: api/Stock
        [Authorize(Roles = "ADMIN")]
        [HttpPost("CreateStock")]
        public async Task<IActionResult> CreateStock([FromBody] StockCreateDTO stockCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Stock data" });

            var stock = _mapper.Map<Stock>(stockCreateDto);
            bool result = await _stockRepo.CreateStock(stock);
            if (!result)
                return Conflict(new { message = "Stock for this blood group already exists." });

            return Ok(new { message = "Stock added successfully." });
        }

        // PUT: api/Stock/{bloodGroup}
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPut("UpdateStock/{bloodGroup}")]
        public async Task<IActionResult> UpdateStock(string bloodGroup, [FromBody] StockUpdateDTO stockUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Stock data" });

            var stock = _mapper.Map<Stock>(stockUpdateDto);
            bool result = await _stockRepo.UpdateStock(bloodGroup, stock);
            if (!result)
                return NotFound(new { message = "Stock Not Found" });

            return Ok(new { message = "Stock Updated Successfully" });
        }

        // DELETE: api/Stock/{bloodGroup}
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("DeleteStock/{bloodGroup}")]
        public async Task<IActionResult> DeleteStock(string bloodGroup)
        {
            bool result = await _stockRepo.DeleteStock(bloodGroup);
            if (!result)
                return NotFound(new { message = "Stock Not Found" });

            return Ok(new { message = "Stock Deleted Successfully" });
        }
    }
}
