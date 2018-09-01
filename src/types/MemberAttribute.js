'use strict';

const ArgumentType = require('../structures/ArgumentType.js');
const ArgumentParsingError = require('../structures/ArgumentParsingError.js');
const UserAttribute = require('../attributes/MemberAttribute.js');

class MemberAttributeArgumentType extends ArgumentType {
    get id() {
        return 'member-attribute';
    }

    parse(val, { client }) {
        const key = val.toLowerCase();
        const memberAttributes = client.master.registry.attributes.filter(
            attribute => attribute instanceof UserAttribute
        );

        if (!memberAttributes.has(key))
            throw new ArgumentParsingError(`Member Attribute '${key}' doesn't exist.`);

        return memberAttributes.get(key);
    }
}

module.exports = MemberAttributeArgumentType;