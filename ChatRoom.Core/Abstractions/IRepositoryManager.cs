using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.Abstractions
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        IUserChatRoomRepository UserChatRooms { get; }
        IMessageRepository Messages { get; }

        Task<int> CompleteAsync();
        int Complete();
        IDbTransaction BeginTransaction();
    }
}
