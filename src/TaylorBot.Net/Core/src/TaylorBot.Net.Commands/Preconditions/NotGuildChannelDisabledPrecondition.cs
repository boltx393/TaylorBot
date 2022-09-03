﻿using Discord;
using System.Threading.Tasks;
using TaylorBot.Net.Core.Logging;

namespace TaylorBot.Net.Commands.Preconditions
{
    public interface IDisabledGuildChannelCommandRepository
    {
        ValueTask<bool> IsGuildChannelCommandDisabledAsync(MessageChannel channel, IGuild guild, CommandMetadata command);
        ValueTask EnableInAsync(MessageChannel channel, IGuild guild, string commandName);
        ValueTask DisableInAsync(MessageChannel channel, IGuild guild, string commandName);
    }

    public class NotGuildChannelDisabledPrecondition : ICommandPrecondition
    {
        private readonly IDisabledGuildChannelCommandRepository _disabledGuildChannelCommandRepository;

        public NotGuildChannelDisabledPrecondition(IDisabledGuildChannelCommandRepository disabledGuildChannelCommandRepository)
        {
            _disabledGuildChannelCommandRepository = disabledGuildChannelCommandRepository;
        }

        public async ValueTask<ICommandResult> CanRunAsync(Command command, RunContext context)
        {
            if (context.Guild == null)
                return new PreconditionPassed();

            var isDisabled = await _disabledGuildChannelCommandRepository.IsGuildChannelCommandDisabledAsync(context.Channel, context.Guild, command.Metadata);

            return isDisabled ?
                new PreconditionFailed(
                    PrivateReason: $"{command.Metadata.Name} is disabled in {context.Channel.Id} on {context.Guild.FormatLog()}",
                    UserReason: new(string.Join('\n', new[] {
                        $"You can't use `{command.Metadata.Name}` because it is disabled in {context.Channel.Mention}.",
                        $"You can re-enable it by typing </command channel-enable:909694280703016991> {command.Metadata.Name}."
                    }))
                ) :
                new PreconditionPassed();
        }
    }
}
