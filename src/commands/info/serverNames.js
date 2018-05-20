'use strict';

const { Paths } = require('globalobjects');

const DiscordEmbedFormatter = require(Paths.DiscordEmbedFormatter);
const Command = require(Paths.Command);
const TimeUtil = require(Paths.TimeUtil);

class ServerNamesCommand extends Command {
    constructor() {
        super({
            name: 'servernames',
            aliases: ['snames', 'guildnames', 'gnames'],
            group: 'info',
            description: 'Gets a list of previous names for a server.',
            examples: ['servernames'],

            args: [
                {
                    key: 'guild',
                    label: 'server',
                    prompt: 'What server would you like to see the names of?',
                    type: 'guild-or-current'
                }
            ]
        });
    }

    async run({ message, client }, { guild }) {
        const { channel } = message;
        const guildNames = await client.master.database.guildNames.getHistory(guild, 10);
        const embed = DiscordEmbedFormatter
            .baseGuildHeader(guild)
            .setDescription(
                guildNames.map(gn => `${TimeUtil.formatSmall(gn.changed_at)} : ${gn.guild_name}`).join('\n')
            );
        return client.sendEmbed(channel, embed);
    }
}

module.exports = ServerNamesCommand;