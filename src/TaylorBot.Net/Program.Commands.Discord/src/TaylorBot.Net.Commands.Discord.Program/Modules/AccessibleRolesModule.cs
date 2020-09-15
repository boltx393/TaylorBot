﻿using Discord;
using Discord.Commands;
using Humanizer;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaylorBot.Net.Commands.Discord.Program.AccessibleRoles.Domain;
using TaylorBot.Net.Commands.Preconditions;
using TaylorBot.Net.Commands.Types;
using TaylorBot.Net.Core.Colors;
using TaylorBot.Net.Core.Embed;

namespace TaylorBot.Net.Commands.Discord.Program.Modules
{
    [RequireInGuild]
    [Name("Roles 🆔")]
    [Group("roles")]
    [Alias("role", "gr")]
    public class AccessibleRolesModule : TaylorBotModule
    {
        private readonly IAccessibleRoleRepository _accessibleRoleRepository;

        public AccessibleRolesModule(IAccessibleRoleRepository accessibleRoleRepository)
        {
            _accessibleRoleRepository = accessibleRoleRepository;
        }

        [Priority(-1)]
        [RequireTaylorBotPermission(GuildPermission.ManageRoles)]
        [Command]
        [Summary("Assigns you a role that set to accessible in this server.")]
        public async Task<RuntimeResult> GetAsync(
            [Summary("What role would you like to get?")]
            [Remainder]
            RoleNotEveryoneArgument<IRole>? role = null
        )
        {
            var member = (IGuildUser)Context.User;
            var embed = new EmbedBuilder().WithUserAsAuthor(member);

            if (role != null)
            {
                if (!member.RoleIds.Contains(role.Role.Id))
                {
                    var accessibleRole = await _accessibleRoleRepository.GetAccessibleRoleAsync(role.Role);
                    if (accessibleRole != null)
                    {
                        var groupInfo = accessibleRole.Group != null ?
                            new
                            {
                                Group = accessibleRole.Group,
                                MemberRolesInSameGroup = member.RoleIds.Intersect(accessibleRole.Group.OtherRoles.Select(r => r.Id)).ToList()
                            } :
                            null;

                        if (groupInfo != null && groupInfo.MemberRolesInSameGroup.Any())
                        {
                            embed
                                .WithColor(TaylorBotColors.ErrorColor)
                                .WithDescription(string.Join('\n', new[] {
                                    $"Sorry, {role.Role.Mention} is part of the '{groupInfo.Group.Name}' group.",
                                    $"You already have {MentionUtils.MentionRole(groupInfo.MemberRolesInSameGroup.First())} which is part of the same group.",
                                    $"Use `{Context.CommandPrefix}roles group` to configure accessible role groups!"
                                }));
                        }
                        else
                        {
                            await member.AddRoleAsync(role.Role, new RequestOptions
                            {
                                AuditLogReason = $"Assigned accessible role on user's request with message id {Context.Message.Id}."
                            });

                            embed
                                .WithColor(TaylorBotColors.SuccessColor)
                                .WithDescription(string.Join('\n', new[] {
                                    $"You now have {role.Role.Mention}. 😊",
                                    $"Use `{Context.CommandPrefix}role drop {role.Role.Name}` to drop it!"
                                }));
                        }
                    }
                    else
                    {
                        embed
                            .WithColor(TaylorBotColors.ErrorColor)
                            .WithDescription(string.Join('\n', new[] {
                                $"Sorry, {role.Role.Mention} is not marked as accessible so I can't give it to you.",
                                $"Use `{Context.CommandPrefix}roles add {role.Role.Name}` to make it accessible to everyone!"
                            }));
                    }
                }
                else
                {
                    embed
                        .WithColor(TaylorBotColors.ErrorColor)
                        .WithDescription(string.Join('\n', new[] {
                            $"You already have role {role.Role.Mention}.",
                            $"Use `{Context.CommandPrefix}role drop {role.Role.Name}` to drop it!"
                        }));
                }
            }
            else
            {
                var accessibleRoles = (await _accessibleRoleRepository.GetAccessibleRolesAsync(Context.Guild))
                    .Where(ar => Context.Guild.Roles.Any(r => r.Id == ar.RoleId.Id))
                    .ToList();

                embed.WithColor(TaylorBotColors.SuccessColor);

                if (accessibleRoles.Any())
                {
                    var ungrouped = accessibleRoles.Where(ar => ar.GroupName == null).ToList();
                    var grouped = accessibleRoles.Where(ar => ar.GroupName != null).GroupBy(ar => ar.GroupName);

                    embed.WithDescription($"Here are the accessible roles in this server, use `{Context.CommandPrefix}role role-name` to get one of them.");

                    if (ungrouped.Any())
                    {
                        embed.AddField("no group", string.Join(", ", ungrouped.Select(r => MentionUtils.MentionRole(r.RoleId.Id))).Truncate(EmbedFieldBuilder.MaxFieldValueLength), inline: false);
                    }

                    foreach (var group in grouped.Take(EmbedBuilder.MaxFieldCount - embed.Fields.Count))
                    {
                        embed.AddField(group.Key, string.Join(", ", group.Select(r => MentionUtils.MentionRole(r.RoleId.Id))).Truncate(EmbedFieldBuilder.MaxFieldValueLength), inline: true);
                    }
                }
                else
                {
                    embed.WithDescription(string.Join('\n', new[] {
                        $"There is currently no accessible role in this server.",
                        $"Accessible roles are roles that everyone has access to using `{Context.CommandPrefix}role`.",
                        $"Use `{Context.CommandPrefix}roles add role-name` to add one!"
                    }));
                }

            }

            return new TaylorBotEmbedResult(embed.Build());
        }

        [RequireTaylorBotPermission(GuildPermission.ManageRoles)]
        [Command("drop")]
        [Summary("Removes an accessible role you currently have.")]
        public async Task<RuntimeResult> DropAsync(
            [Summary("What role would you like to be removed?")]
            [Remainder]
            RoleNotEveryoneArgument<IRole> role
        )
        {
            var member = (IGuildUser)Context.User;
            var embed = new EmbedBuilder().WithUserAsAuthor(member);

            if (member.RoleIds.Contains(role.Role.Id))
            {
                if (await _accessibleRoleRepository.IsRoleAccessibleAsync(role.Role))
                {
                    await member.RemoveRoleAsync(role.Role, new RequestOptions
                    {
                        AuditLogReason = $"Removed accessible role on user's request with message id {Context.Message.Id}."
                    });

                    embed
                        .WithColor(TaylorBotColors.SuccessColor)
                        .WithDescription(string.Join('\n', new[] {
                            $"Removed {role.Role.Mention} from your roles. 😊",
                            $"Use `{Context.CommandPrefix}role {role.Role.Name}` to get it back!"
                        }));
                }
                else
                {
                    embed
                        .WithColor(TaylorBotColors.ErrorColor)
                        .WithDescription(string.Join('\n', new[] {
                            $"Sorry, {role.Role.Mention} is not accessible so you can't drop it.",
                            $"Use `{Context.CommandPrefix}roles add {role.Role.Name}` to make it accessible to everyone!"
                        }));
                }
            }
            else
            {
                embed
                    .WithColor(TaylorBotColors.ErrorColor)
                    .WithDescription($"You don't have the role {role.Role.Mention} so you can't drop it!");
            }

            return new TaylorBotEmbedResult(embed.Build());
        }

        [RequireUserPermissionOrOwner(GuildPermission.ManageRoles)]
        [Command("add")]
        [Summary("Adds a role as accessible to everyone in this server.")]
        public async Task<RuntimeResult> AddAsync(
            [Summary("What role would you like to make accessible?")]
            [Remainder]
            RoleNotEveryoneArgument<IRole> role
        )
        {
            await _accessibleRoleRepository.AddAccessibleRoleAsync(role.Role);

            return new TaylorBotEmbedResult(new EmbedBuilder()
                .WithUserAsAuthor(Context.User)
                .WithColor(TaylorBotColors.SuccessColor)
                .WithDescription(string.Join('\n', new[] {
                    $"Successfully made {role.Role.Mention} accessible to everyone in the server. 😊",
                    $"Use `{Context.CommandPrefix}role {role.Role.Name}` to get it!",
                    $"Use `{Context.CommandPrefix}roles remove {role.Role.Name}` to make it inaccessible again!"
                }))
            .Build());
        }

        [RequireUserPermissionOrOwner(GuildPermission.ManageRoles)]
        [Command("remove")]
        [Summary("Removes a previously accessible role.")]
        public async Task<RuntimeResult> RemoveAsync(
            [Summary("What role would you like to make inaccessible?")]
            [Remainder]
            RoleNotEveryoneArgument<IRole> role
        )
        {
            await _accessibleRoleRepository.RemoveAccessibleRoleAsync(role.Role);

            return new TaylorBotEmbedResult(new EmbedBuilder()
                .WithUserAsAuthor(Context.User)
                .WithColor(TaylorBotColors.SuccessColor)
                .WithDescription(string.Join('\n', new[] {
                    $"Successfully made {role.Role.Mention} inaccessible to everyone in the server. 😊",
                    $"This action did not remove the role from users who already had it.",
                    $"Use `{Context.CommandPrefix}roles` to see remaining accessible roles!"
                }))
            .Build());
        }

        [RequireUserPermissionOrOwner(GuildPermission.ManageRoles)]
        [Command("group")]
        [Summary("Adds an accesible role to a group. Users can only get one accessible role of the same group.")]
        public async Task<RuntimeResult> GroupAsync(
            [Summary("What group would you like to add an accessible role to?")]
            AccessibleGroupName group,
            [Summary("What role would you like to make accessible in the group?")]
            [Remainder]
            RoleNotEveryoneArgument<IRole> role
        )
        {
            var embed = new EmbedBuilder().WithUserAsAuthor(Context.User);

            if (group.Name == "clear")
            {
                await _accessibleRoleRepository.ClearGroupFromAccessibleRoleAsync(role.Role);

                embed
                    .WithColor(TaylorBotColors.SuccessColor)
                    .WithDescription(string.Join('\n', new[] {
                        $"Successfully removed {role.Role.Mention} from its group.",
                        $"Use `{Context.CommandPrefix}roles` to see all accessible roles."
                    }));
            }
            else
            {
                await _accessibleRoleRepository.AddOrUpdateAccessibleRoleWithGroupAsync(role.Role, group);

                embed
                    .WithColor(TaylorBotColors.SuccessColor)
                    .WithDescription(string.Join('\n', new[] {
                        $"Successfully put {role.Role.Mention} in the '{group.Name}' group.",
                        $"Users can only get one accessible role of the same group when using `{Context.CommandPrefix}role`.",
                        $"Use `{Context.CommandPrefix}roles clear {role.Role.Name}` to remove it from the group."
                    }));
            }

            return new TaylorBotEmbedResult(embed.Build());
        }
    }
}
