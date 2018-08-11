using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatRoom.Core.Abstractions.OperationInterfaces;
using Microsoft.AspNetCore.Authorization;
using ChatRoom.Core.Models.ViewModel;
using ChatRoom.Core.Models;
using ChatRoom.Core.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ChatRoom.Core.Entites;

namespace ChatRoom.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserOperations _userOperations;
        private readonly TokenAuthentification _tokenAuthentication;

        public UsersController(IUserOperations userOperations, IOptions<TokenAuthentification> tokenAuthentication)
        {
            _userOperations = userOperations;
            _tokenAuthentication = tokenAuthentication.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<Response> Authenticate([FromBody]LoginViewModel userModel)
        {
            var user = await _userOperations.AuthenticateAsync(userModel.Username, userModel.Password);

            if (user == null)
                return new Response
                {
                    ErrorMessage = "Username or password is incorrect",
                    Status = ResponseStatus.Error,
                };

            var userToken = GenerateUserToken(user);

            return new Response
            {
                Result = new UserAuthenticationModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = userToken
                },
                Status = ResponseStatus.Ok,
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Response> Register([FromBody]RegistrationViewModel userModel)
        {
            var user = new User
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Username = userModel.Username
            };
            await _userOperations.CreateAsync(user, userModel.Password);

            return new Response
            {
                Status = ResponseStatus.Ok
            };
        }

        [HttpGet]
        public async Task<Response> GetAll()
        {
            var users = await _userOperations.GetAllAsync();
            var userModels = users.Select(x => new UserViewModel
            {
                FirstName = x.FirstName,
                Id = x.Id,
                LastName = x.LastName,
                Username = x.Username
            });

            return new Response
            {
                Status = ResponseStatus.Ok,
                Result = userModels
            };
        }

        [HttpGet("{id}")]
        public async Task<Response> GetById(int id)
        {
            var user = await _userOperations.GetByIdAsync(id);

            return new Response
            {
                Result = new UserViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                },
                Status = ResponseStatus.Ok
            };
        }

        [HttpPut("{id}")]
        public async Task<Response> Update(int id, [FromBody]RegistrationViewModel userModel)
        {
            var user = new User
            {
                FirstName = userModel.FirstName,
                Id = id,
                LastName = userModel.LastName,
                Username = userModel.Username
            };

            await _userOperations.UpdateAsync(user, userModel.Password);

            return new Response
            {
                Status = ResponseStatus.Ok
            };
        }

        [HttpDelete("{id}")]
        public async Task<Response> Delete(int id)
        {
            await _userOperations.DeleteAsync(id);
            return new Response
            {
                Status = ResponseStatus.Ok
            };
        }

        #region -- helper functions --
        private string GenerateUserToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenAuthentication.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        #endregion
    }
}