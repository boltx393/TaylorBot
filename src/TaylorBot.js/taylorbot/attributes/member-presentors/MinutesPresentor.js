'use strict';

const moment = require('moment');

const SimpleStatPresentor = require('./SimpleStatPresentor.js');
const DiscordEmbedFormatter = require('../../modules/DiscordEmbedFormatter.js');
const StringUtil = require('../../modules/StringUtil.js');
const MathUtil = require('../../modules/MathUtil.js');

class MinutesPresentor extends SimpleStatPresentor {
    constructor(attribute) {
        super(attribute);
    }

    present(commandContext, member, { [this.attribute.columnName]: minuteCount, rank }) {
        const duration = moment.duration(minuteCount, 'minutes');

        return DiscordEmbedFormatter
            .baseUserEmbed(member.user)
            .setDescription([
                `${member.displayName} has been active for ${StringUtil.plural(minuteCount, 'minute', '**')} in this server.`,
                `This is roughly equivalent to **${duration.humanize()}** of activity.`,
                `They are the **${MathUtil.formatNumberSuffix(rank)}** user of the server (excluding users who left).`
            ].join('\n'));
    }
}

module.exports = MinutesPresentor;
