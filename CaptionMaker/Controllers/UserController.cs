using CaptionMaker.Data.Model;
using CaptionMaker.Data.Repository;
using CaptionMaker.Model;
using CaptionMaker.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaptionMaker.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepo;

        public UserController(JwtService jwtService, IUserRepository userRepo)
        {
            this._jwtService = jwtService;
            this._userRepo = userRepo;
        }

        // TODO: Test user creation
        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UserRequest req)
        {
            if (req == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "No data contained in the request"
                });
            }

            if (String.IsNullOrEmpty(req.Username))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "No username informed"
                });
            }

            if (String.IsNullOrEmpty(req.Password))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "No password informed"
                });
            }

            if (await this._userRepo.UsernameAlreadyExistsAsync(req.Username))
            {
                return Conflict(new ErrorResponse
                {
                    Error = "username already exists"
                });
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(req.Password + salt);

            await this._userRepo.SaveAsync(req.Username, hash, salt);

            return Ok(new SuccessResponse
            {
                Message = "User created successfully"
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest req)
        {
            if (req == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "No data contained in the request"
                });
            }

            if (String.IsNullOrEmpty(req.Username))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "No username informed"
                });
            }

            if (String.IsNullOrEmpty(req.Password))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "No password informed"
                });
            }

            User user = await this._userRepo.GetByUsernameAsync(req.Username);

            if (user == null)
            {
                return Unauthorized(new ErrorResponse
                {
                    Error = $"The user \"{req.Username}\" does not exist"
                });
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(req.Password + user.Salt, user.Password))
            {
                AuthToken authToken = this._jwtService.Generate(user);

                return Ok(authToken);
            }
            else
            {
                return Unauthorized(new ErrorResponse
                {
                    Error = "Incorrect password"
                });
            }
        }
    }
}
