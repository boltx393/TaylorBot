'use strict';

const Attribute = require('./Attribute.js');

class MemberAttribute extends Attribute {
    constructor(options) {
        super(options);
        if (new.target === MemberAttribute) {
            throw new Error(`Can't instantiate abstract ${this.constructor.name} class.`);
        }
    }

    async retrieve(commandContext, member) { // eslint-disable-line no-unused-vars
        throw new Error(`${this.constructor.name} doesn't have a retrieve() method.`);
    }

    async rank(commandContext, guild) { // eslint-disable-line no-unused-vars
        throw new Error(`${this.constructor.name} doesn't have a rank() method.`);
    }
}

module.exports = MemberAttribute;