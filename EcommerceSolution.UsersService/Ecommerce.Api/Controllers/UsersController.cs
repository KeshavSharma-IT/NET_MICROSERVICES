using Ecommerce.Core.DTO;
using Ecommerce.Core.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
               _usersService = usersService;
        }

        // api/Users/{userID}
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetUserByUserID(Guid userID) {

            if (userID == Guid.Empty)
            {
                return BadRequest("Invalid UserID");
            }

           UserDTO? responce= await _usersService.GetUserByUserID(userID);

            if (responce == null)
            {
                return NotFound(responce);
            }

            return Ok(responce);    
        
        }
    }
}
