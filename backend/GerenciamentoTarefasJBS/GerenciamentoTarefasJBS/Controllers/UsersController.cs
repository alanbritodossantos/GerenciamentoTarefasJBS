using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciamentoTarefasJBS.Data;
using GerenciamentoTarefasJBS.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GerenciamentoTarefasJBS.Configurations;
using Microsoft.Extensions.Options;

namespace GerenciamentoTarefasJBS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        public UsersController(ApplicationDbContext context, IConfiguration configuration, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _configuration = configuration;
            _jwtSettings = jwtSettings.Value;
        }

        //a ação GET que retorna todos os usuários
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userParam)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == userParam.Username && x.Password == userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var claims = new[]
            {
               new Claim(ClaimTypes.Name, user.Username),
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_jwtSettings.ValidIssuer, _jwtSettings.ValidAudience, claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }



    }

}
