﻿using TaylorBot.Net.Commands.Discord.Program.Modules.Favorite.Commands;
using TaylorBot.Net.Core.User;

namespace TaylorBot.Net.Commands.Discord.Program.Modules.Favorite.Infrastructure;

public class BaePostgresRepository(TextAttributePostgresRepository textAttributePostgresRepository) : IBaeRepository
{
    public ValueTask<string?> GetBaeAsync(DiscordUser user) =>
        textAttributePostgresRepository.GetAttributeAsync(user, "bae");

    public ValueTask SetBaeAsync(DiscordUser user, string songs) =>
        textAttributePostgresRepository.SetAttributeAsync(user, "bae", songs);

    public ValueTask ClearBaeAsync(DiscordUser user) =>
        textAttributePostgresRepository.ClearAttributeAsync(user, "bae");
}
