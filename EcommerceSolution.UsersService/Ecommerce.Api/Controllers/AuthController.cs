using Ecommerce.Core.DTO;
using Ecommerce.Core.IService;
using Ecommerce.Core.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUsersService _service;
        public AuthController(IUsersService service)
        {
            _service=service;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (registerRequest == null) { return BadRequest("Invalid registration data"); }

           AuthenticationResponse? authenticationResponse= await _service.Register(registerRequest);
            if (authenticationResponse == null || authenticationResponse.Success == false)
            {
                return BadRequest(authenticationResponse);
            }
            return Ok(authenticationResponse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {   var validator=new LoginRequestValidator();
            var ValidationResult=await validator.ValidateAsync(loginRequest);
            if(!ValidationResult.IsValid) return BadRequest(ValidationResult.Errors.Select(e=>e.ErrorMessage));

            //if (loginRequest == null) { return BadRequest("Invalid Login Data"); }

          AuthenticationResponse? authenticationResponse  = await _service.Login(loginRequest);

            if (authenticationResponse == null || authenticationResponse.Success == false)
            {
                return BadRequest(authenticationResponse);
            }

            return Ok(authenticationResponse);



        }

    }
}
