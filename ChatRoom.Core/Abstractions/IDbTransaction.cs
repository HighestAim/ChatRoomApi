using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Core.Abstractions
{
    public interface IDbTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
