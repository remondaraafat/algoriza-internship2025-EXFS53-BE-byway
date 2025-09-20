using APICoursePlatform.Helpers;
using APICoursePlatform.Models;
using Application.DTOs.AccountDTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Register.Commands
{
    public class GoogleRegisterCommand : IRequest<GeneralResponse<List<string>>>
    {
        public GoogleRegisterDTO DTO { get; set; }
    }

    public class GoogleRegisterCommandHandler : IRequestHandler<GoogleRegisterCommand, GeneralResponse<List<string>>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public GoogleRegisterCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<GeneralResponse<List<string>>> Handle(GoogleRegisterCommand request, CancellationToken cancellationToken)
        {
            var existEmail = await userManager.FindByEmailAsync(request.DTO.Email);
            if (existEmail != null)
                return GeneralResponse<List<string>>.FailResponse("Email already exists", new List<string> { "This email is already in use." });

            var user = new ApplicationUser
            {
                UserName = request.DTO.Email,
                Email = request.DTO.Email,
                FullName = request.DTO.FullName,
                EmailConfirmed = true // since Google already verified
            };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                List<string> errorList = result.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to create user", errorList);
            }

            var role = await userManager.AddToRoleAsync(user, request.DTO.UserRole);
            if (!role.Succeeded)
            {
                await userManager.DeleteAsync(user); // rollback
                List<string> roleErrors = role.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to assign role", roleErrors);
            }

            return GeneralResponse<List<string>>.SuccessResponse("Google user registered successfully");
        }
    }
}
