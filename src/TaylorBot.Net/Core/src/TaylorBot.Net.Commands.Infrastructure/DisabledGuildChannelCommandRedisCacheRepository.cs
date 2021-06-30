﻿using Discord;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using TaylorBot.Net.Commands.Preconditions;

namespace TaylorBot.Net.Commands.Infrastructure
{
    public class DisabledGuildChannelCommandRedisCacheRepository : IDisabledGuildChannelCommandRepository
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly DisabledGuildChannelCommandPostgresRepository _disabledGuildChannelCommandPostgresRepository;

        public DisabledGuildChannelCommandRedisCacheRepository(ConnectionMultiplexer connectionMultiplexer, DisabledGuildChannelCommandPostgresRepository disabledGuildChannelCommandPostgresRepository)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _disabledGuildChannelCommandPostgresRepository = disabledGuildChannelCommandPostgresRepository;
        }

        public async ValueTask<bool> IsGuildChannelCommandDisabledAsync(MessageChannel channel, IGuild guild, CommandMetadata command)
        {
            var redis = _connectionMultiplexer.GetDatabase();
            var key = $"enabled-commands:guild:{guild.Id}:channel:{channel.Id}";
            var isEnabled = await redis.HashGetAsync(key, command.Name);

            if (!isEnabled.HasValue)
            {
                var isDisabled = await _disabledGuildChannelCommandPostgresRepository.IsGuildChannelCommandDisabledAsync(channel, guild, command);
                await redis.HashSetAsync(key, command.Name, !isDisabled);
                await redis.KeyExpireAsync(key, TimeSpan.FromHours(6));
                return isDisabled;
            }

            return !(bool)isEnabled;
        }
    }
}
