﻿using Dapper;
using Discord;
using System.Threading.Tasks;
using TaylorBot.Net.Commands.Discord.Program.Modules.Mod.Domain;
using TaylorBot.Net.Core.Infrastructure;
using TaylorBot.Net.Core.Snowflake;

namespace TaylorBot.Net.Commands.Discord.Program.Modules.Mod.Infrastructure
{
    public class ModLogChannelPostgresRepository : IModLogChannelRepository
    {
        private readonly PostgresConnectionFactory _postgresConnectionFactory;

        public ModLogChannelPostgresRepository(PostgresConnectionFactory postgresConnectionFactory)
        {
            _postgresConnectionFactory = postgresConnectionFactory;
        }

        public async ValueTask AddOrUpdateModLogAsync(ITextChannel textChannel)
        {
            using var connection = _postgresConnectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                @"INSERT INTO moderation.mod_log_channels (guild_id, channel_id)
                VALUES (@GuildId, @ChannelId)
                ON CONFLICT (guild_id) DO UPDATE SET
                    channel_id = excluded.channel_id;",
                new
                {
                    GuildId = textChannel.GuildId.ToString(),
                    ChannelId = textChannel.Id.ToString()
                }
            );
        }

        public async ValueTask RemoveModLogAsync(IGuild guild)
        {
            using var connection = _postgresConnectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "DELETE FROM moderation.mod_log_channels WHERE guild_id = @GuildId;",
                new
                {
                    GuildId = guild.Id.ToString()
                }
            );
        }

        private class LogChannelDto
        {
            public string channel_id { get; set; } = null!;
        }

        public async ValueTask<ModLog?> GetModLogForGuildAsync(IGuild guild)
        {
            using var connection = _postgresConnectionFactory.CreateConnection();

            var logChannel = await connection.QuerySingleOrDefaultAsync<LogChannelDto?>(
                @"SELECT channel_id FROM moderation.mod_log_channels
                WHERE guild_id = @GuildId;",
                new
                {
                    GuildId = guild.Id.ToString()
                }
            );

            return logChannel != null ? new ModLog(new SnowflakeId(logChannel.channel_id)) : null;
        }
    }
}
