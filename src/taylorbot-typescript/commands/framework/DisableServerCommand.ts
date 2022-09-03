import { Command } from '../Command';
import { CommandMessageContext } from '../CommandMessageContext';

class DisableServerCommandCommand extends Command {
    constructor() {
        super({
            name: 'disableservercommand',
            aliases: ['disableguildcommand', 'dgc', 'dsc'],
            group: 'framework',
            description: 'This command has been removed. Please use </command server-disable:909694280703016991> instead.',
            examples: [''],

            args: [
                {
                    key: 'args',
                    label: 'args',
                    type: 'any-text',
                    prompt: 'What arguments would you like to use?'
                }
            ]
        });
    }

    async run({ message, client }: CommandMessageContext, { args }: { args: string }): Promise<void> {
        await client.sendEmbedError(message.channel, [
            `This command has been removed.`,
            `Please use </command server-disable:909694280703016991> instead.`
        ].join('\n'));
    }
}

export = DisableServerCommandCommand;
