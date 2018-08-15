'use strict';

const chrono = require('chrono-node');

const ArgumentType = require('../structures/ArgumentType.js');
const ArgumentParsingError = require('../structures/ArgumentParsingError.js');

class FutureTimeArgumentType extends ArgumentType {
    constructor() {
        super({
            includesSpaces: true
        });
    }

    get id() {
        return 'future-time';
    }

    parse(val) {
        const results = chrono.parse(val, new Date(), { forwardDate: true });

        if (results.length > 0) {
            const moment = results[0].start.moment();

            if (moment.isAfter(Date.now())) {
                return moment;
            }
        }

        throw new ArgumentParsingError(`Could not parse future date from '${val}'.`);
    }
}

module.exports = FutureTimeArgumentType;