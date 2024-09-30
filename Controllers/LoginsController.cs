using Assetmanagementsystem.Models;
using Assetmanagementsystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Assetmanagementsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private IConfiguration _config;
        private readonly ILoginRepository _loginRepository;

        public LoginsController(ILoginRepository loginRepository,IConfiguration config)
        {
            _config = config;   
            _loginRepository = loginRepository; 
        }
        [HttpGet("{username}/{password}")]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Variables for tracking unauthorized
            IActionResult response = Unauthorized(); //401
            Login dbUser = null;

            // 1 - Authenticate the user by passing username and password
            dbUser = await _loginRepository.ValidateUser(username, password);

            // 2 - Generate JWT Token
            if (dbUser != null)
            {
                // Custom Method for generating token
                var tokenString = GenerateJWTToken(dbUser);

                response = Ok(new
                {
                    userName = dbUser.Username,
                    userType = dbUser.Usertype,
                    loginId = dbUser.LoginId,   
                    token = tokenString,
                });
            }
            return response;
        }


        private string GenerateJWTToken(Login dbUser)
        {
            // 1- Secret Security Key
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // 2- Algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //3- JWT
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials);
            //4- Writing Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register ([FromBody] UserRegistration user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newUser = await _loginRepository.RegisterUserAsync(user);

                if (newUser != null)
                {
                    return Ok(newUser);
                }
                return NotFound("User could not be created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _loginRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(); // User not found
                }

                return Ok(user); // Return the user data
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown here for brevity)
                return StatusCode(500, "An error occurred while retrieving user information."); // Internal server error
            }
        }


        [HttpPut("update/{uId}")]
        public async Task<IActionResult> UpdateUser(int uId, [FromBody] UserRegistration user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedUser = await _loginRepository.UpdateUserAsync(uId, user);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Database update error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("assets")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<AssetMaster>>> GetAllassets()
        {
            var asset = await _loginRepository.GetAllAssets();
            if (asset == null || !asset.Any())
            {
                return NotFound("No asset found");
            }
            return Ok(asset);
        }

        // GET: api/asset/{id}
        [HttpGet("assetbyid/{id}")]
        public async Task<IActionResult> GetAssetById(int id)
        {
            var asset = await _loginRepository.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound($"Asset with ID {id} not found.");
            }
            return Ok(asset);
        }


        [HttpPost("A")]
        public async Task<ActionResult<AssetMaster>> InsertNewAssetReturnRecord(AssetMaster asset)
        {
            try
            {
                //Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    //return the validation errors
                    return BadRequest(ModelState);
                }
                //Insert a new record and return the object named employee
                var newAsset = await _loginRepository.InsertAssetReturnRecord(asset);

                //if the employee object is successfully created,return 201 created
                if (newAsset != null)
                {
                    return Ok(newAsset);
                }

                //if the employee could not be inserted, return notfound
                return NotFound("Asset could not be created");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error:{ex.Message}");
            }
        }




        // GET: api/asset/search?term={searchTerm}
        [HttpGet("searchbyid")]
        public async Task<ActionResult<IEnumerable<AssetMaster>>> SearchAssets(string searchTerm)
        {
            var assets = await _loginRepository.SearchAssetsAsync(searchTerm);
            return Ok(assets);
        }

        // GET: api/<LoginsController>
        /* [HttpGet]
         public IEnumerable<string> Get()
         {
             return new string[] { "value1", "value2" };
         }*/

        /* // GET api/<LoginsController>/5
         [HttpGet("{id}")]
         public string Get(int id)
         {
             return "value";
         }
 */
        // POST api/<LoginsController>
        /*[HttpPost]
        public void Post([FromBody] string value)
        {
        }*/


        // PUT api/<LoginsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
