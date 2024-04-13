using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }
        //  this functionality to get Messages outside next to lists of likes in UI
        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var MessageQuery = _context.Messages
                    .OrderByDescending(x => x.MessageSent)
                    .AsQueryable();

            MessageQuery = messageParams.Container switch
            {
                "Inbox"  => MessageQuery.Where(u => u.RecipientUserName == messageParams.username 
                                                && u.RecipientDeleted == false ),
                "Outbox" => MessageQuery.Where(u => u.SenderUserName == messageParams.username
                                                && u.SenderDeleted == false),
                _ => MessageQuery.Where(u => u.RecipientUserName == messageParams.username
                                        && u.RecipientDeleted == false && u.DateRead == null)
            };

            var messages = MessageQuery.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageSize, messageParams.PageNumber);

        }
        //  this functionality to get Message thread between 2 users inside
        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentuserName, string recipientName)
        {
            var messages = await _context.Messages
                 .Include(u => u.Sender).ThenInclude(p => p.Photos)
                 .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                 .Where(
                     u => u.RecipientUserName == currentuserName && u.RecipientDeleted == false &&
                     u.SenderUserName == recipientName ||
                     u.SenderUserName== currentuserName &&  u.SenderDeleted== false &&
                     u.RecipientUserName == recipientName
                ).OrderBy(x => x.MessageSent)
                .ToListAsync();

            var unReadMessages = messages.Where(m=> m.DateRead == null && m.RecipientUserName == currentuserName).ToList();

            if (unReadMessages.Any())
            {
                foreach (var message in unReadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);

        }

        public async Task<bool> SaveAllAsync()
        {
          return await _context.SaveChangesAsync() > 0;
        }
    }
}
