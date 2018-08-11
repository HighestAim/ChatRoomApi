using ChatRoom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Core.Abstractions.RepositoryInterfaces
{
    public interface IUserRepository : IEntityBaseRepository<User>
    {
    }
}
