'use strict';

const Command = require('../Command.js');
const CommandError = require('../CommandError.js');
const Imgur = require('../../modules/imgur/ImgurModule.js');
const DiscordEmbedFormatter = require('../../modules/DiscordEmbedFormatter.js');

class ImgurCommand extends Command {
    constructor() {
        super({
            name: 'imgur',
            group: 'imgur',
            description: `Upload a picture on Imgur! If it's not already uploaded to a website, you can add it as an attachment to your command.`,
            examples: ['https://www.example.com/link/to/picture.jpg'],

            args: [
                {
                    key: 'url',
                    label: 'url',
                    type: 'url-or-attachment',
                    prompt: `What's the link to the picture you would you like upload?`
                }
            ]
        });
    }

    async run({ message, client }, { url }) {
        const { author, channel } = message;

        const response = await Imgur.upload(url);

        if (!response.success)
            throw new CommandError(`Something went wrong when uploading to Imgur. 😕`);

        const { link } = response.data;

        return client.sendEmbed(channel, DiscordEmbedFormatter
            .baseUserEmbed(author)
            .setDescription(`Successfully uploaded your image to Imgur, it can be found here: ${link} 😊.`)
            .setImage(link)
        );
    }
}

module.exports = ImgurCommand;