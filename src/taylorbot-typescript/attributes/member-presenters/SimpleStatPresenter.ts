import { DiscordEmbedFormatter } from '../../modules/discord/DiscordEmbedFormatter';
import { StringUtil } from '../../modules/util/StringUtil';
import { Format } from '../../modules/discord/DiscordFormatter';
import { MemberAttributePresenter } from '../MemberAttributePresenter';
import { GuildMember, MessageEmbed } from 'discord.js';
import { SimpleStatMemberAttribute } from '../SimpleStatMemberAttribute';
import { MemberAttribute } from '../MemberAttribute';
import { CommandMessageContext } from '../../commands/CommandMessageContext';
import { MathUtil } from '../../modules/util/MathUtil';

export class SimpleStatPresenter implements MemberAttributePresenter {
    protected attribute: SimpleStatMemberAttribute;

    constructor(attribute: MemberAttribute) {
        if (!(attribute instanceof SimpleStatMemberAttribute))
            throw new Error('Attribute must be simple stat.');
        this.attribute = attribute as SimpleStatMemberAttribute;
    }

    present(commandContext: CommandMessageContext, member: GuildMember, { [this.attribute.columnName]: stat, rank }: Record<string, any> & { rank: string }): MessageEmbed {
        return DiscordEmbedFormatter
            .baseUserSuccessEmbed(member.user)
            .setDescription([
                `${member.displayName} has ${StringUtil.plural(stat, this.attribute.singularName, '**')}.`,
                `They are the **${MathUtil.formatNumberSuffix(Number.parseInt(rank))}** user of the server (excluding users who left).`
            ].join('\n'));
    }

    presentRankEntry(member: GuildMember, { [this.attribute.columnName]: stat, rank }: Record<string, any> & { rank: string }): string {
        return `${rank}: ${Format.escapeDiscordMarkdown(member.user.username)} - ${StringUtil.plural(stat, this.attribute.singularName, '`')}`;
    }
}
