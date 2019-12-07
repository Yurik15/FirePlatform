using AutoMapper;
using FirePlatform.Models.Containers;
using FirePlatform.Models.Models;
using FirePlatform.Services;
using FirePlatform.WebApi.Model.Requests;
using FirePlatform.WebApi.Model.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Controllers
{
    [ApiController]
    public class AccountController : BaseController
    {
        public AccountController
            (
                Service service,
                IMapper mapper,
                IConfiguration config
            )
            : base(service, mapper)
        {
            _config = config;
        }

        #region Authorization

        private IConfiguration _config;

        [HttpPost("api/[controller]/Login")]
        [EnableCors("AllowAll")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserRequest request)
        {
            var userId = LoginTest(request);

            if (userId != null)
            {
                var tokenString = GenerateJSONWebToken();
                return Ok(new { token = tokenString, userId });
            }

            return Ok();
        }

        private string GenerateJSONWebToken()
        {
            string tokenToReturn = string.Empty;
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  null,
                  expires: DateTime.Now.AddHours(24),
                  signingCredentials: credentials);

                tokenToReturn = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return tokenToReturn;
            }
            return tokenToReturn;
        }

        [HttpPost("api/[controller]/Register")]
        [ProducesResponseType(201, Type = typeof(UserResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserResponse>> Register([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = Mapper.Map<UserRequest, Users>(request);
            var userFromDbContainer = await Service.GetUserService().Register(user);

            if (userFromDbContainer.Message == null)
            {
                var response = Mapper.Map<UserResponse>(user);
                var container = new ApiContainer<UserResponse>
                {
                    DataObject = response,
                };

                return Ok(container);
            }
            else
            {
                var response = Mapper.Map<UserResponse>(user);
                var container = new ApiContainer<UserResponse>
                {
                    DataObject = response,
                    Message = userFromDbContainer.Message
                };

                return BadRequest(container);
            }
        }

        /* TODO 
         * Merge this with existing method with same name after db server deploying
        [HttpPost("api/[controller]/Login")]
        [ProducesResponseType(201, Type = typeof(UserResponse))]
        [ProducesResponseType(400)]
        public ActionResult<UserResponse> Login([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = Mapper.Map<UserRequest, User>(request);
            bool success = Service.GetUserService().ValidUserCredentials(user.Login, user.Password);

            if (success)
            {
                var response = Mapper.Map<UserResponse>(user);
                var container = new ApiContainer<UserResponse>
                {
                    DataObject = response
                };

                return Ok(container);
            }
            else
            {
                var response = Mapper.Map<UserResponse>(user);
                var container = new ApiContainer<UserResponse>
                {
                    DataObject = response,
                    Message = "Wrong login or password"
                };

                return BadRequest(container);
            }
        }
        */

        private int? LoginTest([FromBody] UserRequest request)
        {
            var fakeCredentials = new List<UserRequest>()
            {
                new UserRequest()
                {
                    Id = 1,
                    Login = "toffix",
                    Password = "passPiotr%"
                             },
                new UserRequest()
                {
                    Id = 2,
                    Login = "doman",
                    Password = "passTomek%"
                },
                new UserRequest()
                {
                    Id = 3,
                    Login = "sitar",
                    Password = "passPiotr%"
                },
                new UserRequest()
                {
                    Id = 4,
                    Login = "kidyb",
                    Password = "passBartek%"
                },
                new UserRequest()
                {
                    Id = 5,
                    Login = "user1",
                    Password = "pass234%"
                },
                new UserRequest()
                {
                      Id = 6,
                      Login = "user3",
                      Password = "pass456%"
                },
                new UserRequest()
                {
                     Id = 7,
                    Login = "user5",
                    Password = "pass678%"
                },
                new UserRequest()
                {
                    Id = 8,
                    Login = "user7",
                    Password = "pass8910%"
                },
                new UserRequest()
                {
                    Id = 9,
                    Login = "admin",
                    Password = "admin"
                }
            };
            var fakeCredential = fakeCredentials.FirstOrDefault(x => x.Login == request.Login && x.Password == request.Password);

            return fakeCredential?.Id;
        }

        [HttpPost("api/[controller]/Users")]
        [ProducesResponseType(201, Type = typeof(UserResponse))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserResponse>> Add([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = Mapper.Map<UserRequest, Users>(request);
            var result = await Service.GetUserService().Create(user);
            var response = Mapper.Map<UserResponse>(result);
            var container = new ApiContainer<UserResponse>
            {
                DataObject = response
            };

            return Ok(container);
        }

        #endregion

        #region Users CRUD
        [HttpGet("api/[controller]/Users/{id}")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Users>> Get(int id)
        {
            var result = await Service.GetUserService().GetById(id);
            var response = Mapper.Map<UserResponse>(result);

            if (result == null)
                return NotFound();
            return Ok(response);
        }

        [HttpPut("api/[controller]/Users")]
        [ProducesResponseType(201, Type = typeof(UserResponse))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserResponse>> Update([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = Mapper.Map<UserRequest, Users>(request);
            var result = await Service.GetUserService().Update(user);
            var response = Mapper.Map<UserResponse>(result);
            var container = new ApiContainer<UserResponse>
            {
                DataObject = response
            };

            return Ok(container);
        }

        [HttpDelete("api/[controller]/UsersDel/{id}")]
        public async Task Delete(int id)
        {
            await Service.GetUserService().Delete(id);
        }

        [HttpGet("api/[controller]/Users")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiContainer<UserResponse>>> GetAll()
        {
            var result = await Service.GetUserService().Get();
            if (result == null)
                return NotFound();

            var response = Mapper.Map<IEnumerable<Users>, IEnumerable<UserResponse>>(result);
            var container = new ApiContainer<UserResponse>
            {
                DataCollection = response
            };

            return Ok(container);
        }
        #endregion
    }
}
