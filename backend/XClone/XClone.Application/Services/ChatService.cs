using System.Security.Claims;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Requets.Chat;
using XClone.Application.Models.Responses;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Database.SqlServer.Entities;
using XClone.Domain.Exceptions;
using XClone.Shared.Constants;

namespace XClone.Application.Services;

public class ChatService(IUnitOfWork uow, IUserService userService) : IChatService
{
    public async Task<GenericResponse<ChatDto>> SendMessage(SendMessageRequest model, Claim claim)
    {
        var sender = await userService.GetExecutor(claim.Value);
        var receiver = await uow.userRepository.Get(model.ReceiverId)
            ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);

        if (sender.Id == receiver.Id)
            throw new BadRequestException("No puedes enviarte mensajes a ti mismo");

        var userLow = sender.Id.CompareTo(receiver.Id) < 0 ? sender.Id : receiver.Id;
        var userHigh = sender.Id.CompareTo(receiver.Id) < 0 ? receiver.Id : sender.Id;

        var chat = await uow.chatRepository.GetByUsers(userLow, userHigh);
        if (chat is null)
        {
            chat = new Chat
            {
                UserLowId = userLow,
                UserHighId = userHigh
            };
            chat = await uow.chatRepository.Create(chat);
        }

        var message = new Message
        {
            ChatId = chat.Id,
            SenderId = sender.Id,
            Texto = model.Texto
        };
        await uow.messageRepository.Create(message);
        await uow.SaveChangesAsync();

        return ResponseHelper.Create(MapChat(chat));
    }

    public async Task<GenericResponse<List<MessageDto>>> GetMessages(Guid chatId, int limit, int offset, Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var chat = await uow.chatRepository.Get(chatId)
            ?? throw new NotFoundException("Chat no encontrado");

        if (chat.UserLowId != executor.Id && chat.UserHighId != executor.Id)
            throw new UnauthorizedException("No tienes acceso a este chat");

        var messages = await uow.messageRepository.GetChatMessages(chatId, limit, offset);
        var dtos = messages.Select(MapMessage).ToList();
        return ResponseHelper.Create(dtos);
    }

    public async Task<GenericResponse<List<ChatDto>>> GetMyChats(Claim claim)
    {
        var executor = await userService.GetExecutor(claim.Value);
        var chats = await uow.chatRepository.GetUserChats(executor.Id);
        var dtos = new List<ChatDto>();

        foreach (var chat in chats)
        {
            var dto = MapChat(chat);
            var lastMsg = (await uow.messageRepository.GetChatMessages(chat.Id, 1, 0)).FirstOrDefault();
            if (lastMsg != null)
                dto.LastMessage = MapMessage(lastMsg);
            dtos.Add(dto);
        }

        return ResponseHelper.Create(dtos);
    }

    private static ChatDto MapChat(Chat chat)
    {
        return new ChatDto
        {
            Id = chat.Id,
            UserLowId = chat.UserLowId,
            UserHighId = chat.UserHighId,
            CreatedAt = chat.CreatedAt
        };
    }

    private static MessageDto MapMessage(Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            ChatId = message.ChatId,
            SenderId = message.SenderId,
            Texto = message.Texto,
            CreatedAt = message.CreatedAt
        };
    }
}
