﻿using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessageController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageController(IUserRepository userRepository ,
                                IMessageRepository messageRepository ,
                                IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage( CreateMessageDto createMessageDto)
        {
            // get user name  of sender  from claims 
            var SenderUserName = User.GetUsername();

            // if you target yourself with this message 
            if (SenderUserName == createMessageDto.RecipientUserName)
                return BadRequest("you can`t message yourself ");

            // get sender as object of AppUser 
            var sender = await _userRepository.GetUserByUserNameAsync(SenderUserName);

            // get recipient as object of AppUser 
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null) 
                return NotFound();
            // fill message object with proper data 
            var message = new Message()
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) 
                return Ok(_mapper.Map<MessageDto>(message));  // if succeeded =>  pass from Message Object to MessageDTO

            return BadRequest("Failed to Save Message"); // if ==> Failed to save to DB => return bad request
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessageForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.username = User.GetUsername();


            var messages = await _messageRepository.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(
                new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.RecordsCount, messages.PagesCount)
                );

            return messages ;

        }
    }
}
