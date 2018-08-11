using ChatRoom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using ChatRoom.Core.Models.ViewModel;
using System.Threading.Tasks;

namespace ChatRoom.Core.Abstractions.RepositoryInterfaces
{
    public interface IMessageRepository : IEntityBaseRepository<Message>
    {
        Task<IEnumerable<MessageViewModel>> GetByChatRoomId(int chatRoomId);
    }
}
