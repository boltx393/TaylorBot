﻿using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaylorBot.Net.Commands.Events;
using TaylorBot.Net.Commands.PostExecution;
using TaylorBot.Net.Commands.Types;
using TaylorBot.Net.Core.Program.Events;
using TaylorBot.Net.Core.Program.Extensions;
using TaylorBot.Net.Core.Tasks;

namespace TaylorBot.Net.Commands.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddTaylorBotApplicationServices(configuration)
                .AddSingleton(services)
                .AddSingleton(provider => new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async
                }))
                .AddTransient<SingletonTaskRunner>()
                .AddTransient<IUserMessageReceivedHandler, CommandHandler>()
                .AddTransient<IAllReadyHandler, CommandHandler>()
                .AddSingleton<PageMessageReactionsHandler>()
                .AddTransient<IReactionAddedHandler>(c => c.GetRequiredService<PageMessageReactionsHandler>())
                .AddTransient<IReactionRemovedHandler>(c => c.GetRequiredService<PageMessageReactionsHandler>())
                .AddTransient<CommandExecutedHandler>()
                .AddTransient<CommandServiceLogger>()
                .AddTransient<IUserTracker, UserTracker>()
                .AddTransient<MentionedUserTypeReader<IUser>>()
                .AddTransient<CustomUserTypeReader<IUser>>()
                .AddTransient<MentionedUserTypeReader<IGuildUser>>()
                .AddTransient<CustomUserTypeReader<IGuildUser>>()
                .AddTransient<MentionedUserNotAuthorTypeReader<IGuildUser>>()
                .AddTransient<MentionedUserNotAuthorTypeReader<IUser>>()
                .AddTransient<MentionedUsersNotAuthorTypeReader<IUser>>()
                .AddTransient<MentionedUserNotAuthorOrClientTypeReader<IGuildUser>>()
                .AddTransient<CustomRoleTypeReader<IRole>>()
                .AddTransient<RoleNotEveryoneTypeReader<IRole>>()
                .AddTransient<CustomChannelTypeReader<IChannel>>()
                .AddTransient<PositiveInt32.Factory>()
                .AddTransient<WordTypeReader>()
                .AddTransient<CommandTypeReader>();
        }
    }
}
