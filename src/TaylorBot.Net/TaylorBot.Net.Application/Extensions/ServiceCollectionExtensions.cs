﻿using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaylorBot.Net.Application.Options;
using TaylorBot.Net.Core.Client;
using TaylorBot.Net.Core.Configuration;
using TaylorBot.Net.Core.Logging;

namespace TaylorBot.Net.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTaylorBotApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<DiscordOptions>(configuration.GetSection("Discord"))
                .AddTransient<ILogSeverityToLogLevelMapper, LogSeverityToLogLevelMapper>()
                .AddTransient(provider => new TaylorBotToken(provider.GetRequiredService<IOptionsMonitor<DiscordOptions>>().CurrentValue.Token))
                .AddTransient(provider => new DiscordSocketConfig { TotalShards = (int)provider.GetRequiredService<IOptionsMonitor<DiscordOptions>>().CurrentValue.ShardCount })
                .AddTransient(provider => new DiscordShardedClient(provider.GetRequiredService<DiscordSocketConfig>()))
                .AddSingleton<TaylorBotClient>();
        }

        public static IServiceCollection AddTaylorBotApplicationLogging(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddLogging(configure => configure.AddConsole().AddConfiguration(configuration.GetSection("Logging")));
        }
    }
}
