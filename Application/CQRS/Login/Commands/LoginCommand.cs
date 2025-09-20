﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using APICoursePlatform.DTOs.AccountDTOs;
using APICoursePlatform.Helpers;
using APICoursePlatform.Models;

namespace APICoursePlatform.CQRS.Login.Commands
{
    public class LoginCommand : IRequest<GeneralResponse<string>>
    {
        public LoginDTO DTO { get; set; }

        public LoginCommand(LoginDTO dto)
        {
            DTO = dto;
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, GeneralResponse<string>>
    {
        public readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public LoginCommandHandler(UserManager<ApplicationUser> UserManager, IConfiguration Configuration)
        {
            userManager = UserManager;
            config = Configuration;
        }

        public async Task<GeneralResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(request.DTO.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, request.DTO.Password))
                return GeneralResponse<string>.FailResponse("Invalid email or password.");

            


            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim("username", user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecritKey"]));
            var signingCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);
            var expiresAt =  DateTime.Now.AddDays(7) ;

            //Design token
            var token = new JwtSecurityToken(
                issuer: config["JWT:IssuerIP"],
                audience: config["JWT:AudienceIP"],
                expires: expiresAt,
                claims: claims,
                signingCredentials: signingCredentials
            );

            //generate token
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return GeneralResponse<string>.SuccessResponse("Login successful", jwtToken);
        }
    }


}

