﻿//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using APICoursePlatform.CQRS.Notification.Commands;
//using APICoursePlatform.CQRS.Notification.Queries;
//using APICoursePlatform.DTOs.NotificationDTO;

//namespace APICoursePlatform.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class NotificationController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public NotificationController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        #region Notification

//        [HttpGet("MyNotifications")]
//        public async Task<IActionResult> GetUserNotifications(int pageNumber = 1, int pageSize = 10)
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


//            var result = await _mediator.Send(new GetUserNotificationsQuery(userId, pageNumber, pageSize));

//            return result.Success ? Ok(result) : BadRequest(result);
//        } 
//        #endregion

//        //[HttpPut("MarkAsRead/{notificationId:int}")]
//        //public async Task<IActionResult> MarkAsRead(int notificationId)
//        //{
//        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//        //    var command = new MarkNotificationAsReadCommand(userId, notificationId);
//        //    var result = await _mediator.Send(command);

//        //    return result.Success ? Ok(result) : BadRequest(result);
//        //}

//        [HttpPut("GetUnreadNotificationsCount")]
//        public async Task<IActionResult> GetUnreadNotificationsCount()
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            var result = await _mediator.Send(new GetUnreadNotificationsCountQuery(userId));

//            return result.Success ? Ok(result) : BadRequest(result);
//        }
//    }
//}
