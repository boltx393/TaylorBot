'use strict';

const UserAttribute = require('./UserAttribute.js');
const DiscordEmbedFormatter = require('../modules/DiscordEmbedFormatter.js');

class SimpleUserTextAttribute extends UserAttribute {
    constructor(options) {
        super(options);
        if (new.target === SimpleUserTextAttribute) {
            throw new Error(`Can't instantiate abstract ${this.constructor.name} class.`);
        }
    }

    async retrieve({ client }, user) {
        const attribute = await client.master.database.textAttributes.get(this.id, user);

        if (!attribute) {
            return DiscordEmbedFormatter
                .baseUserHeader(user)
                .setColor('#f04747')
                .setDescription(`${user.username}'s ${this.description} is not set. 🚫`);
        }
        else {
            return DiscordEmbedFormatter
                .baseUserEmbed(user)
                .setTitle(`${user.username}'s ${this.description}`)
                .setDescription(attribute.attribute_value);
        }
    }
}

module.exports = SimpleUserTextAttribute;