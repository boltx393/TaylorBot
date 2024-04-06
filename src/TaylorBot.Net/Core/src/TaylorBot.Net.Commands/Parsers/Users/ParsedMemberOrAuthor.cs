﻿using Discord;
using OperationResult;
using System.Text.Json;
using TaylorBot.Net.Core.Client;
using static OperationResult.Helpers;

namespace TaylorBot.Net.Commands.Parsers.Users;

public record ParsedMemberOrAuthor(IGuildUser Member);

public class MemberOrAuthorParser(MemberParser memberParser, ITaylorBotClient taylorBotClient) : IOptionParser<ParsedMemberOrAuthor>
{
    public async ValueTask<Result<ParsedMemberOrAuthor, ParsingFailed>> ParseAsync(RunContext context, JsonElement? optionValue, Interaction.Resolved? resolved)
    {
        if (optionValue.HasValue)
        {
            var parsedUser = await memberParser.ParseAsync(context, optionValue, resolved);
            if (parsedUser)
            {
                return new ParsedMemberOrAuthor(parsedUser.Value.Member);
            }
            else
            {
                return Error(parsedUser.Error);
            }
        }
        else
        {
            var user = context.FetchedUser ?? await FetchUserAsync(context);

            if (user is not IGuildUser member)
            {
                return Error(new ParsingFailed("Member option can only be used in a server."));
            }

            return new ParsedMemberOrAuthor(member);
        }
    }

    private async Task<IUser> FetchUserAsync(RunContext context)
    {
        return context.Guild != null
            ? await taylorBotClient.ResolveGuildUserAsync(context.Guild.Id, context.User.Id) ?? await taylorBotClient.ResolveRequiredUserAsync(context.User.Id)
            : await taylorBotClient.ResolveRequiredUserAsync(context.User.Id);
    }
}
