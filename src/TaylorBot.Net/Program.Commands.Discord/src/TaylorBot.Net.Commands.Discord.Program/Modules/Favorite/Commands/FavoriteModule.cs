﻿using Discord;
using Discord.Commands;
using TaylorBot.Net.Commands.DiscordNet;
using TaylorBot.Net.Commands.Types;
using TaylorBot.Net.Core.Embed;

namespace TaylorBot.Net.Commands.Discord.Program.Modules.Favorite.Commands;

[Name("Favorite")]
public class FavoriteModule(
    ICommandRunner commandRunner,
    FavoriteSongsShowSlashCommand favoriteSongsShowCommand,
    FavoriteSongsSetSlashCommand favoriteSongsSetCommand,
    FavoriteBaeShowSlashCommand favoriteBaeShowCommand,
    FavoriteObsessionShowSlashCommand favoriteObsessionShowCommand) : TaylorBotModule
{
    [Command("fav")]
    [Alias("favsongs", "favoritesongs")]
    [Summary("Show the favorite songs of a user")]
    public async Task<RuntimeResult> ShowFavAsync(
        [Summary("What user would you like to see the favorite songs of?")]
        [Remainder]
        IUserArgument<IUser>? user = null
    )
    {
        var u = user == null ? Context.User : await user.GetTrackedUserAsync();

        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(
            favoriteSongsShowCommand.Show(new(u)),
            context
        );

        return new TaylorBotResult(result, context);
    }

    [Command("setfav")]
    [Alias("set fav", "setfavsongs", "set favsongs")]
    [Summary("Register your favorite songs for others to see")]
    public async Task<RuntimeResult> SetFavAsync(
        [Remainder]
        string text
    )
    {
        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(
            favoriteSongsSetCommand.Set(context.User, text, null),
            context
        );

        return new TaylorBotResult(result, context);
    }

    [Command("clearfav")]
    [Alias("clear fav", "clearfavsongs", "clear favsongs", "clearfavoritesongs", "clear favoritesongs")]
    [Summary("This command has been moved to </favorite songs clear:1169468169140838502>. Please use it instead! 😊")]
    public async Task<RuntimeResult> ClearFavAsync(
        [Remainder]
        string? _ = null
    )
    {
        var command = new Command(
            DiscordNetContextMapper.MapToCommandMetadata(Context),
            () => new(new EmbedResult(EmbedFactory.CreateError(
                """
                This command has been moved to 👉 </favorite songs clear:1169468169140838502> 👈
                Please use it instead! 😊
                """))));

        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(command, context);

        return new TaylorBotResult(result, context);
    }

    [Command("bae")]
    [Summary("Show the bae of a user")]
    public async Task<RuntimeResult> ShowBaeAsync(
        [Summary("What user would you like to see the bae of?")]
        [Remainder]
        IUserArgument<IUser>? user = null
    )
    {
        var u = user == null ? Context.User : await user.GetTrackedUserAsync();

        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(
            favoriteBaeShowCommand.Show(new(u)),
            context
        );

        return new TaylorBotResult(result, context);
    }

    [Command("setbae")]
    [Alias("set bae")]
    [Summary("This command has been moved to </favorite bae set:1169468169140838502>. Please use it instead! 😊")]
    public async Task<RuntimeResult> SetBaeAsync(
        [Remainder]
        string? _ = null
    )
    {
        var command = new Command(
            DiscordNetContextMapper.MapToCommandMetadata(Context),
            () => new(new EmbedResult(EmbedFactory.CreateError(
                """
                This command has been moved to 👉 </favorite bae set:1169468169140838502> 👈
                Please use it instead! 😊
                """))));

        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(command, context);

        return new TaylorBotResult(result, context);
    }

    [Command("clearbae")]
    [Summary("This command has been moved to </favorite bae clear:1169468169140838502>. Please use it instead! 😊")]
    public async Task<RuntimeResult> ClearBaeAsync(
        [Remainder]
        string? _ = null
    )
    {
        var command = new Command(
            DiscordNetContextMapper.MapToCommandMetadata(Context),
            () => new(new EmbedResult(EmbedFactory.CreateError(
                """
                This command has been moved to 👉 </favorite bae clear:1169468169140838502> 👈
                Please use it instead! 😊
                """))));

        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(command, context);

        return new TaylorBotResult(result, context);
    }

    [Command("waifu")]
    [Summary("Show the obsession of a user")]
    public async Task<RuntimeResult> ShowWaifuAsync(
        [Summary("What user would you like to see the obsession of?")]
        [Remainder]
        IUserArgument<IUser>? user = null
    )
    {
        var u = user == null ? Context.User : await user.GetTrackedUserAsync();

        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(
            favoriteObsessionShowCommand.Show(u),
            context
        );

        return new TaylorBotResult(result, context);
    }

    [Command("setwaifu")]
    [Alias("set waifu")]
    [Summary("This command has been moved to </favorite obsession set:1169468169140838502>. Please use it instead! 😊")]
    public async Task<RuntimeResult> SetWaifuAsync(
        [Remainder]
        string? _ = null
    )
    {
        var command = new Command(
            DiscordNetContextMapper.MapToCommandMetadata(Context),
            () => new(new EmbedResult(EmbedFactory.CreateError(
                """
                This command has been moved to 👉 </favorite obsession set:1169468169140838502> 👈
                Please use it instead! 😊
                """))));

        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(command, context);

        return new TaylorBotResult(result, context);
    }

    [Command("clearwaifu")]
    [Alias("clear waifu")]
    [Summary("This command has been moved to </favorite obsession clear:1169468169140838502>. Please use it instead! 😊")]
    public async Task<RuntimeResult> ClearWaifuAsync(
        [Remainder]
        string? _ = null
    )
    {
        var command = new Command(
            DiscordNetContextMapper.MapToCommandMetadata(Context),
            () => new(new EmbedResult(EmbedFactory.CreateError(
                """
                This command has been moved to 👉 </favorite obsession clear:1169468169140838502> 👈
                Please use it instead! 😊
                """))));

        var context = DiscordNetContextMapper.MapToRunContext(Context);
        var result = await commandRunner.RunAsync(command, context);

        return new TaylorBotResult(result, context);
    }
}
