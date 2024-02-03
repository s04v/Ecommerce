using Common.Data;
using Common.Exceptions;
using Core.Auth.Dtos;
using Core.Auth.Events;
using Core.Users.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;
using Core.Auth.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMediator _mediator;

        public AuthService(IApplicationDbContext dbContext, IMediator mediator, IConfiguration config)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _config = config;
        }

        public async Task<string> LoginAsync(LoginDto form)
        {
            var user = await _dbContext.User
                .Where(o => o.Email == form.Email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new DomainException("Email or password is wrong");
            }

            bool passwordVerify = BCryptNet.Verify(form.Password, user.Password);

            if (!passwordVerify)
            {
                throw new DomainException("Email or password is wrong");
            }

            var jwtToken = GenerateJWT(user);

            var loginEvent = new SuccessLoginEvent { 
                UserUuid = user.Uuid,
                IpAddress = form.IpAddress,
                Device = form.UserDevice,
            };

            await _mediator.Publish(loginEvent);

            return jwtToken;
        }

        public async Task RegisterAsync(RegisterDto form)
        {
            var user = await _dbContext.User
               .Where(o => o.Email == form.Email)
               .FirstOrDefaultAsync();

            if (user != null)
            {
                throw new DomainException("User already exists");
            }

            string passwordHash = BCryptNet.HashPassword(form.Password, 10);
            
            var newUser = new User
            {
                Email = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName,
                Password = passwordHash
            };

            await _dbContext.User
                .AddAsync(newUser);

            await _dbContext.SaveChangesAsync();
        }

        private string GenerateJWT(User user)
        {
            var key = _config.GetValue<string>("Jwt:Key");
            var issuer = _config.GetValue<string>("Jwt:Issuer");
            var audience = _config.GetValue<string>("Jwt:Audience");

            if (string.IsNullOrEmpty(key))
            {
                throw new DomainException("Internal error");
            }

            var claims = new List<Claim>
            {
                new Claim("id", user.Uuid.ToString()),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                notBefore: DateTime.UtcNow,
                claims: claimsIdentity.Claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
