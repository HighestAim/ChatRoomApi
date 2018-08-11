using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.DAL.Repositories
{
    public class UserChatRoomRepository : EntityBaseRepository<UserChatRoom>, IUserChatRoomRepository
    {
        public UserChatRoomRepository(ChatRoomContext context) : base(context)
        {
        }
    }
}
