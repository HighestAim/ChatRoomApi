using ChatRoom.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Core.Entites
{
    public class User : EntityBase
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
