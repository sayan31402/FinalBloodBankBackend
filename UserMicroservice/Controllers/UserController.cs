using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMicroservice.Models.DTO;
using UserMicroservice.Models;
using UserMicroservice.Services.DAO;
using Microsoft.AspNetCore.Authorization;

namespace UserMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserController(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }



        // POST: api/User
        [Authorize(Roles = "ADMIN")]
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] UserCreateDTO userCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid User data" });

            var user = _mapper.Map<User>(userCreateDto);
            bool result = await _userRepo.CreateUser(user);
            if (!result)
                return StatusCode(500, new { message = "Failed to add new User" });

            return Ok(new { message = "User added successfully." });
        }

        // GET: api/User
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            //var userDtos = _mapper.Map<IEnumerable<UserGetDTO>>(users);
            //return Ok(userDtos);
            return Ok(users);
        }

        // GET: api/User/{id}
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetUserById/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepo.GetUserById(id);
            if (user == null)
                return NotFound(new { message = "User Not Found" });

            var userDto = _mapper.Map<UserGetDTO>(user);
            return Ok(userDto);
        }

        // GET: api/User/by-username/{username}
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpGet("GetUserByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null)
                return NotFound(new { message = "User Not Found" });

            var userDto = _mapper.Map<UserGetDTO>(user);
            return Ok(userDto);
        }

        // PUT: api/User/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpPut("UpdateUser/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid User data" });

            var user = _mapper.Map<User>(userUpdateDto);
            bool result = await _userRepo.UpdateUser(id, user);
            if (!result)
                return NotFound(new { message = "User Not Found" });

            return Ok(new { message = "User Updated Successfully" });
        }

        // DELETE: api/User/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("DeleteUser/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool result = await _userRepo.DeleteUser(id);
            if (!result)
                return NotFound(new { message = "User Not Found" });

            return Ok(new { message = "User Deleted Successfully" });
        }
    }
}
