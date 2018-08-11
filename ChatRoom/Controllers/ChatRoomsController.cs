using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatRoom.Core.Abstractions.OperationInterfaces;
using Microsoft.AspNetCore.Authorization;
using ChatRoom.Core.Models.ViewModel;
using ChatRoom.Core.Models;
using ChatRoom.Core.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ChatRoom.Core.Entites;

namespace ChatRoom.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChatRoomsController : Controller
    {
        private readonly IUserChatRoomOperations _chatRoomOperations;

        public ChatRoomsController(IUserChatRoomOperations chatRoomOperations)
        {
            _chatRoomOperations = chatRoomOperations;
        }

        [HttpGet("{id}")]
        public async Task<Response> GetById(int id)
        {
            var chatRoom = await _chatRoomOperations.GetByIdAsync(id);

            return new Response
            {
                Result = new ChatRoomViewModel
                {
                    Id = chatRoom.Id,
                    Name = chatRoom.Name,
                },
                Status = ResponseStatus.Ok
            };
        }

        [HttpGet("{id}/Messages")]
        public async Task<Response> GetMessages(int id)
        {
            var messages = await _chatRoomOperations.GetMessages(id);

            return new Response
            {
                Result = messages,
                Status = ResponseStatus.Ok
            };
        }

        [HttpGet]
        public async Task<Response> GetAll()
        {
            var chatRoomList = await _chatRoomOperations.GetAllAsync();

            return new Response
            {
                Result = chatRoomList.Select(chatRoom => new ChatRoomViewModel
                {
                    Id = chatRoom.Id,
                    Name = chatRoom.Name,
                }),
                Status = ResponseStatus.Ok
            };
        }

        [HttpPost]
        public async Task<Response> AddChatRoom([FromBody]string Name)
        {
            var chatRoom = await _chatRoomOperations.CreateAsync(new UserChatRoom {  Name = Name });

            return new Response
            {
                Result = new ChatRoomViewModel
                {
                    Id = chatRoom.Id,
                    Name = chatRoom.Name,
                },
                Status = ResponseStatus.Ok
            };
        }

       
    }
}