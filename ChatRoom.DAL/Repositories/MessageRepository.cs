using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using ChatRoom.Core.Models.ViewModel;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.DAL.Repositories
{
    public class MessageRepository : EntityBaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ChatRoomContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MessageViewModel>> GetByChatRoomId(int chatRoomId)
        {
            return await Context.Messages.Where(x => x.ChatRoomId == chatRoomId)
                                         .Select(x => new MessageViewModel
                                         {
                                             ChatRoomId = x.ChatRoomId,
                                             MessageText = x.MessageText,
                                             SentDate = x.SentTime,
                                             UserId = x.SenderId,
                                             UserName = x.Sender.FirstName
                                         }).ToListAsync();
        }                                
    }
}
