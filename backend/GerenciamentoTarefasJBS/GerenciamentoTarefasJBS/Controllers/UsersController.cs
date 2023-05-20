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
    [Authorize]
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


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            // Verificar se o ID fornecido corresponde ao ID do usuário a ser atualizado.
            if (id != user.Id)
            {
                // Retornar um erro 400 se o ID não corresponder.
                return BadRequest();
            }

            // Informar ao contexto do EF Core que o usuário foi modificado.
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                // Tentar salvar as alterações no banco de dados.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Se ocorrer um erro de concorrência, verificar se o usuário ainda existe.
                if (!UserExists(id))
                {
                    // Se o usuário não existir, retornar um erro 404.
                    return NotFound();
                }
                else
                {
                    // Se ocorrer outro tipo de erro, lançar a exceção para ser tratada em um nível superior.
                    throw;
                }
            }

            // Se tudo correr bem e o usuário for atualizado com sucesso, retornar um código de status 204.
            return NoContent();
        }

        // Método auxiliar para verificar se um usuário existe no banco de dados.
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }




        [AllowAnonymous] //não exije autenticação
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
