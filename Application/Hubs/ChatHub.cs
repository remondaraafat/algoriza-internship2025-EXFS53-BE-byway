//using MediatR;
//using Microsoft.AspNetCore.SignalR;
//using APICoursePlatform.CQRS.ChatCQRS.Commands;
//using APICoursePlatform.DTOs.ChatDTOs;

//namespace APICoursePlatform.Hubs
//{
//    public class ChatHub:Hub
//    {
//        private readonly IMediator _mediator;
//        public ChatHub(IMediator mediator)
//        {
//            _mediator = mediator;
//        }
//        public Task SendMessage(SendChatMessageRequestDTO DTO)
//        {
//            return _mediator.Send(new SendChatMessageCommand{DTO = DTO});
//        }
//    }
//}
