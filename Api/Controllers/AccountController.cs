using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration.GetSection("JWT:ValidIssuer").Value,
                    audience: _configuration.GetSection("JWT:ValidAudience").Value,
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)


                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                    ,expiration = token.ValidTo
            });
            }
            var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Oops!!!" };
            //throw new HttpResponseException(msg);
            return Unauthorized(msg);
        }


        //search
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> Search(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return await _context.customerInfo.Where(s => s.Name.Contains(id) || s.Email.Contains(id) || s.Phone.Contains(id)).OrderByDescending(s => s.Name).ToListAsync();
            }
            else
            {
                return Ok();
            }
        }
    }
}
