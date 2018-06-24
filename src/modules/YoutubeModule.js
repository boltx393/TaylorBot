'use strict';

const fetch = require('node-fetch');
const querystring = require('querystring');
const { MessageEmbed } = require('discord.js');
const { Paths } = require('globalobjects');

const { googleAPIKey } = require(Paths.GoogleConfig);
const StringUtil = require(Paths.StringUtil);

class YoutubeModule {
    static async getLatestVideo(playlistId) {
        const body = await fetch(`https://www.googleapis.com/youtube/v3/playlistItems?${querystring.stringify({
            'key': googleAPIKey,
            'part': 'snippet',
            'playlistId': playlistId
        })}`).then(res => res.json());

        const video = body.items[0].snippet;
        return video;
    }

    static getEmbed(video) {
        const re = new MessageEmbed({
            'title': StringUtil.shrinkString(video.title, 65, ' ...'),
            'description': StringUtil.shrinkString(video.description, 200, ' ...'),
            'url': `https://youtu.be/${video.resourceId.videoId}`,
            'timestamp': new Date(video.publishedAt),
            'author': {
                'name': video.channelTitle,
                'url': `https://www.youtube.com/channel/${video.channelId}`
            },
            'footer': {
                'text': 'YouTube',
                'icon_url': 'http://i.imgur.com/ZQUERxd.png'
            },
            'thumbnail': video.thumbnails.medium,
            'color': 0xe52d27
        });

        return re;
    }
}

module.exports = YoutubeModule;