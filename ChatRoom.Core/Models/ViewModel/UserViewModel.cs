﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Core.Models.ViewModel
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}
