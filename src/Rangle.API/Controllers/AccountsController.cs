using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rangle.Abstractions.Entities;
using Rangle.Abstractions.Services;
using Rangle.API.Models;

namespace Rangle.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private IAccountService _userService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get(CancellationToken cancellationToken = default)
        {
            IEnumerable<UserEntity> users = await _userService.GetUsers(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<UserModel>>(users));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserModel>> Get(int id, CancellationToken cancellationToken = default)
        {
            UserEntity user = await _userService.GetUser(id, cancellationToken);
            return _mapper.Map<UserModel>(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register([FromBody] UserRegisterModel userRegisterModel, CancellationToken cancellationToken = default)
        {
            UserEntity user = _mapper.Map<UserEntity>(userRegisterModel);

            UserEntity signedUpUser = await _userService.Register(user, cancellationToken);

            return CreatedAtAction("Get", new { id = signedUpUser.Id }, _mapper.Map<UserModel>(signedUpUser));
        }
    }
}
