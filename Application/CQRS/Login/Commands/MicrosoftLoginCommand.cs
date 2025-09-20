using APICoursePlatform.Helpers;
using APICoursePlatform.Models;
using Application.DTOs.AccountDTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Login.Commands
{
    public class MicrosoftLoginCommand : IRequest<GeneralResponse<string>>
    {
        public MicrosoftLoginDTO DTO { get; set; }

        public MicrosoftLoginCommand(MicrosoftLoginDTO dto)
        {
            DTO = dto;
        }
    }

    public class MicrosoftLoginCommandHandler : IRequestHandler<MicrosoftLoginCommand, GeneralResponse<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public MicrosoftLoginCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        public async Task<GeneralResponse<string>> Handle(MicrosoftLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.DTO.Email);
            if (user == null)
                return GeneralResponse<string>.FailResponse("User not found. Please register first.");

            // هنا مفيش Password check, هنفترض إن ProviderKey جاي Verified من Microsoft
            if (string.IsNullOrEmpty(request.DTO.ProviderKey))
                return GeneralResponse<string>.FailResponse("Invalid Microsoft token");

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("username", user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecritKey"]));
            var signingCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.Now.AddDays(7);

            var token = new JwtSecurityToken(
                issuer: config["JWT:IssuerIP"],
                audience: config["JWT:AudienceIP"],
                expires: expiresAt,
                claims: claims,
                signingCredentials: signingCredentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return GeneralResponse<string>.SuccessResponse("Microsoft login successful", jwtToken);
        }
    }
}
