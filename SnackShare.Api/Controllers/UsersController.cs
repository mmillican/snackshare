using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackShare.Api.Data;
using SnackShare.Api.Data.Entities;
using SnackShare.Api.Mappers;
using SnackShare.Api.Models.Users;

namespace SnackShare.Api.Controllers
{
    [Authorize]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly SnackDbContext _dbContext;
        private readonly ILogger _logger;

        public UsersController(SnackDbContext dbContext,
            ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var model = user.ToModel();

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user by ID: {id}");
                return StatusCode(500, "Error getting user by ID");
            }
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var user = await GetUserByEmail(email);
                if (user == null)
                {
                    return NotFound();
                }

                var model = user.ToModel();

                return Ok(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error getting user by email: {email}");
                return StatusCode(500, "Error getting user by email");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await GetUserByEmail(model.EmailAddress);
                if (user != null)
                {
                    _logger.LogInformation($"User with email '{model.EmailAddress}' already exists");

                    ModelState.AddModelError(nameof(model.EmailAddress), "Email address already exists");
                    return BadRequest(ModelState);
                }

                user = new User();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.EmailAddress = model.EmailAddress;

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                model.Id = user.Id;

                return Created(Url.Action(nameof(GetById), new { id = user.Id }), model);
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Error creating user with email: {model.EmailAddress}");
                return StatusCode(500, "Error creating user");
            }
        }

        private Task<User> GetUserByEmail(string email)
        {
            var user = _dbContext.Users.SingleOrDefaultAsync(x => x.EmailAddress == email);
            return user;
        }
    }
}