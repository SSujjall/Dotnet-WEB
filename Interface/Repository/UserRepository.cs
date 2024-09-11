using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WEB.DTOs;
using WEB.Interface.IRepository;
using WEB.Models;
using WEB.Models.Common;

namespace WEB.Interface.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;


        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyDbCon");
            _configuration = configuration;
        }

        public async Task<bool> FindUser(string username)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                var cmd = new SqlCommand("FindUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Username", username);

                var reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await con.CloseAsync();
                    return true;
                }

                await con.CloseAsync();
                return false;
            }
        }

        public async Task<Response> GetUserById(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                var cmd = new SqlCommand("getUserById", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", id);

                var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var user = new UserDisplayDTO
                    {
                        Username = reader["Username"].ToString()
                    };

                    return new Response
                    {
                        Data = new
                        {
                            username = user.Username
                        },
                        StatusCode = HttpStatusCode.OK
                    };
                }

                return null;
                
            }
        }

        public async Task<Response> UserLogin(UserDTO model)
        {
            User user = null;

            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                var cmd = new SqlCommand("FindUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Username", model.Username);

                var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    user = new User
                    {
                        UserId = (int)reader["UserId"],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString()
                    };
                }
            }


            if (user != null)
            {
                string hashedPassword = HashPassword(model.Password);

                if (hashedPassword == user.Password)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Username", user.Username), // User name claim
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.WriteToken(token);

                    return new Response
                    {
                        Data = new
                        {
                            token = jwtToken
                        },
                        StatusCode = HttpStatusCode.OK
                    };
                }

                return null;
            }

            return null;
        }

        public async Task UserRegister(UserDTO model)
        {
            var user = new User
            {
                Username = model.Username,
                Password = HashPassword(model.Password)
            };

            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                var cmd = new SqlCommand("insertUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        protected string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
