﻿using Discord;
using System.Threading.Tasks;
using TaylorBot.Net.Core.Program.Events;
using TaylorBot.Net.Core.Tasks;
using TaylorBot.Net.MessageLogging.Domain;

namespace TaylorBot.Net.UserNotifier.Program.Events
{
    public class MessageDeletedHandler : IMessageDeletedHandler
    {
        private readonly TaskExceptionLogger _taskExceptionLogger;
        private readonly MessageDeletedLoggerService _messageDeletedLoggerService;

        public MessageDeletedHandler(TaskExceptionLogger taskExceptionLogger, MessageDeletedLoggerService messageDeletedLoggerService)
        {
            _taskExceptionLogger = taskExceptionLogger;
            _messageDeletedLoggerService = messageDeletedLoggerService;
        }

        public ValueTask MessageDeletedAsync(Cacheable<IMessage, ulong> cachedMessage, Cacheable<IMessageChannel, ulong> channel)
        {
            Task.Run(async () => await _taskExceptionLogger.LogOnError(
                _messageDeletedLoggerService.OnMessageDeletedAsync(cachedMessage, await channel.GetOrDownloadAsync()),
                nameof(_messageDeletedLoggerService.OnMessageDeletedAsync)
            ));
            return default;
        }
    }
}
