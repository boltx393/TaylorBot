import fsWithCallbacks = require('fs');
const fs = fsWithCallbacks.promises;
import path = require('path');

import { Log } from '../../tools/Logger';
import { MessageWatcher } from '../../structures/MessageWatcher';
import { TaylorBotClient } from '../TaylorBotClient';
import { Message } from 'discord.js';

const watchersPath = path.join(__dirname, '..', '..', 'watchers');

export class MessageWatcherRegistry {
    #watchers = new Map<string, MessageWatcher>();

    private static requireWatcher(watcherName: string): new () => MessageWatcher {
        return require(path.join(watchersPath, watcherName));
    }

    async loadAll(): Promise<void> {
        const files = await fs.readdir(watchersPath);
        files.forEach(filename => {
            const filePath = path.parse(filename);
            if (filePath.ext === '.js') {
                const Watcher = MessageWatcherRegistry.requireWatcher(filePath.base);
                const watcher = new Watcher();
                if (watcher.enabled)
                    this.#watchers.set(filePath.name, watcher);
            }
        });
    }

    feedAll(client: TaylorBotClient, message: Message): void {
        this.#watchers.forEach(async (watcher, name) => {
            try {
                await watcher.messageHandler(client, message);
            }
            catch (e) {
                if (e instanceof Error) {
                    Log.error(`Message Watcher ${name} Error: \n${e.stack}`);
                }
                else {
                    Log.error(`Message Watcher ${name} Error: \n${e}`);
                }
            }
        });
    }

    getWatcher(name: string): MessageWatcher | undefined {
        return this.#watchers.get(name);
    }
}
