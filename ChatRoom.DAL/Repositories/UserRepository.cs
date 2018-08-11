using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.DAL.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(ChatRoomContext context) : base(context)
        {
        }
    }
}
