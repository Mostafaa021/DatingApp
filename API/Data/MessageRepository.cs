using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using AutoMapper.QueryableExtensions;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context , IMapper mapper)
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
            return  await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var MessageQuery = _context.Messages
                    .OrderByDescending(x=>x.MessageSent) 
                    .AsQueryable();

            MessageQuery = messageParams.Container switch
            {
                "Inbox" => MessageQuery.Where(u => u.RecipientUserName == messageParams.username),
                "Outbox" => MessageQuery.Where(u => u.SenderUserName == messageParams.username),
                _ => MessageQuery.Where(u => u.RecipientUserName == messageParams.username && u.DateRead == null)
            };

            var messages = MessageQuery.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageSize, messageParams.PageNumber);

        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(int currentuserid, int recipientid)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
          return await _context.SaveChangesAsync() > 0;
        }
    }
}
