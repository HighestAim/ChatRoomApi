using ChatRoom.Core.Entites;
using ChatRoom.Core.Models;
using ChatRoom.Core.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.Abstractions.OperationInterfaces
{
    public interface IUserChatRoomOperations
    {
        Task<IEnumerable<UserChatRoom>> GetAllAsync();
        Task<UserChatRoom> GetByIdAsync(int id);
        MessageViewModel AddMessage(MessageViewModel message);
        Task<UserChatRoom> CreateAsync(UserChatRoom chatRoom);
        Task<IEnumerable<MessageViewModel>> GetMessages(int id);
    }
}
