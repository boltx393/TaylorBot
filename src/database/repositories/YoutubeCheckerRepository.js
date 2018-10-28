'use strict';

const Log = require('../../tools/Logger.js');

class YoutubeCheckerRepository {
    constructor(db) {
        this._db = db;
    }

    async getAll() {
        try {
            return await this._db.checkers.youtube_checker.find();
        }
        catch (e) {
            Log.error(`Getting Youtube Channels: ${e}`);
            throw e;
        }
    }

    async update(playlistId, guildId, channelId, lastVideoId) {
        try {
            return await this._db.instance.oneOrNone(
                `UPDATE checkers.youtube_checker SET last_video_id = $[last_video_id]
                WHERE playlist_id = $[playlist_id] AND guild_id = $[guild_id] AND channel_id = $[channel_id]
                RETURNING *;`,
                {
                    'last_video_id': lastVideoId,
                    'playlist_id': playlistId,
                    'guild_id': guildId,
                    'channel_id': channelId
                }
            );
        }
        catch (e) {
            Log.error(`Updating Youtube for guild ${guildId}, channel ${channelId}, playlistId ${playlistId}: ${e}`);
            throw e;
        }
    }
}

module.exports = YoutubeCheckerRepository;