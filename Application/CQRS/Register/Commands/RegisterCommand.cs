using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
//using APICoursePlatform.CQRS.Notification.Commands;
using APICoursePlatform.DTOs.NotificationDTO;
using APICoursePlatform.Helpers;
using APICoursePlatform.Hubs;
using APICoursePlatform.Models;
using APICoursePlatform.UnitOfWorkContract;
using static APICoursePlatform.Enums.Enums;
using APICoursePlatform.DTOs.AccountDTOs;

namespace APICoursePlatform.CQRS.Register.Commands
{
    public class RegisterCommand : IRequest<GeneralResponse<List<string>>>
    {
        
       public RegisterDTO DTO { get; set; }
       
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, GeneralResponse<List<string>>>
    {
        private readonly UserManager<ApplicationUser> UserManager;
        //private readonly IUnitOfWork UnitOfWork;
        private readonly IEmailService _emailService;
        private readonly IMediator mediator;
        private readonly IHubContext<NotificationHub> hubContext;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IEmailService emailService, IMediator mediator, IHubContext<NotificationHub> hubContext)
        {
            this.UserManager = userManager;
           // this.UnitOfWork = unitOfWork;
            this._emailService = emailService;
            this.mediator = mediator;
            this.hubContext = hubContext;
        }

        public async Task<GeneralResponse<List<string>>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
           
            var existEmail = await UserManager.FindByEmailAsync(request.DTO.Email);
            if (existEmail != null)
                return GeneralResponse<List<string>>.FailResponse("Email already exists", new List<string> { "This email is already in use." });

            var user = new ApplicationUser
            {
                UserName = request.DTO.UserName,
                Email = request.DTO.Email,
                FullName = request.DTO.FirstName+" "+ request.DTO.LastName,                
            };

            //create account
            var result = await UserManager.CreateAsync(user, request.DTO.Password);
            if (!result.Succeeded)
            {
                List<string> errorList = result.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to create user", errorList);
            }

            //add role
            var role = await UserManager.AddToRoleAsync(user, request.DTO.UserRole.ToString());
            if (!role.Succeeded)
            {
                await UserManager.DeleteAsync(user);//rollback
                List<string> roleErrors = role.Errors.Select(e => e.Description).ToList();
                return GeneralResponse<List<string>>.FailResponse("Failed to assign role", roleErrors);
            }

           

            
            //await UnitOfWork.SaveAsync();

           // var admins = await UserManager.GetUsersInRoleAsync("Admin");

            //var notification = new CreateNotificationDTO
            //{
            //    Title = "New Startup Account Requires Approval",
            //    Message = $"{request.CompanyName} just signed up as a startup and is waiting for your approval.",
            //    RecipientIds = admins.Select(u => u.Id),
            //    NotificationType = NotificationType.General
            //};

            // 1) Notifications
            //await mediator.Send(new CreateNotificationCommand(notification), cancellationToken);

            // 2)  SignalR
          //  await hubContext.Clients.Users(admins.Select(a => a.Id)).SendAsync("NewApprovalRequest", notification, cancellationToken);


            //email to user

            await _emailService.SendEmailAsync(
                toEmail: request.DTO.Email,
                subject: "Welcome to Course Platform!",
                body: $@"
                    <div style='max-width:600px;margin:auto;font-family:Arial;padding:30px;
                                background:#f9f9f9;border-radius:10px;border:1px solid #ddd;color:#333'>
                        <h1 style='color:#2a7ae2;text-align:center'>Welcome to Course Platform! 🚀</h1>
                        <p style='font-size:16px'>Hi <strong>{request.DTO.FirstName}</strong>,</p>
                        <p>Your account has been created.</p>
                        <hr style='margin:30px 0;border:none;border-top:1px solid #eee'>
                        <footer style='font-size:13px;color:#888;text-align:center'>
                            © {DateTime.Now.Year} Course Platform. All rights reserved.
                        </footer>
                    </div>"
                );

            return GeneralResponse<List<string>>.SuccessResponse("User registered successfully");

        }
    }
}
