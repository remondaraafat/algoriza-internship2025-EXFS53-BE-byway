using APICoursePlatform.CQRS.Login.Commands;
using APICoursePlatform.CQRS.Register.Commands;
using APICoursePlatform.DTOs.AccountDTOs;
using Application.CQRS.Login.Commands;
using Application.CQRS.Register.Commands;
using Application.DTOs.AccountDTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace APICoursePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        #region Account

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO userFromRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
            }

            var result = await _mediator.Send(new RegisterCommand { DTO = userFromRequest });

            if (!result.Success)
                return BadRequest(result);

            var loginResult = await _mediator.Send(new LoginCommand(
                new LoginDTO { Email = userFromRequest.Email, Password = userFromRequest.Password }
            ));

            if (loginResult.Success)
                return Ok(GeneralResponse<object>.SuccessResponse("Registered & Logged in", loginResult));

            return StatusCode(StatusCodes.Status500InternalServerError,
                GeneralResponse<string>.FailResponse("User registered but auto-login failed", loginResult.Message));
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userFromRequest)
        {
            if (ModelState.IsValid)
            {

                var result = await _mediator.Send(new LoginCommand(userFromRequest));

                return result.Success ? Ok(result) : Unauthorized(result);

            }
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return BadRequest(GeneralResponse<List<string>>.FailResponse("Validation Failed", errors));
        }
        #endregion
        //google
        //[HttpPost("google-register")]
        //public async Task<IActionResult> GoogleRegister([FromBody] GoogleRegisterDTO dto)
        //{
        //    var result = await _mediator.Send(new GoogleRegisterCommand { DTO = dto });
        //    return Ok(result);
        //}

        //[HttpPost("google-login")]
        //public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO dto)
        //{
        //    var result = await _mediator.Send(new GoogleLoginCommand(dto));
        //    return Ok(result);
        //}
        ////facebook
        //[HttpPost("facebook-register")]
        //public async Task<IActionResult> FacebookRegister([FromBody] FacebookRegisterDTO dto)
        //{
        //    var result = await _mediator.Send(new FacebookRegisterCommand { DTO = dto });
        //    return Ok(result);
        //}

        //[HttpPost("facebook-login")]
        //public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginDTO dto)
        //{
        //    var result = await _mediator.Send(new FacebookLoginCommand(dto));
        //    return Ok(result);
        //}
        ////microsoft
        //[HttpPost("microsoft-register")]
        //public async Task<IActionResult> MicrosoftRegister([FromBody] MicrosoftRegisterDTO dto)
        //{
        //    var result = await _mediator.Send(new MicrosoftRegisterCommand { DTO = dto });
        //    return Ok(result);
        //}

        //[HttpPost("microsoft-login")]
        //public async Task<IActionResult> MicrosoftLogin([FromBody] MicrosoftLoginDTO dto)
        //{
        //    var result = await _mediator.Send(new MicrosoftLoginCommand(dto));
        //    return Ok(result);
        //}



    }
}
