import * as pgPromise from 'pg-promise';

const pgp = pgPromise({
    capSQL: true
});

import PostgreSQLConfig = require('../config/postgresql.json');

import { UserDAO } from './dao/UserDAO';

import { GuildRepository } from './repositories/GuildRepository';
import { UserRepository } from './repositories/UserRepository';
import { GuildMemberRepository } from './repositories/GuildMemberRepository';
import { UsernameRepository } from './repositories/UsernameRepository';
import { GuildNameRepository } from './repositories/GuildNameRepository';
import { GuildCommandRepository } from './repositories/GuildCommandRepository';
import { CommandRepository } from './repositories/CommandRepository';
import { UserGroupRepository } from './repositories/UserGroupRepository';
import { RoleGroupRepository } from './repositories/RoleGroupRepository';
import { TextChannelRepository } from './repositories/TextChannelRepository';
import { AttributeRepository } from './repositories/AttributeRepository';
import { TextAttributeRepository } from './repositories/TextAttributeRepository';
import { RollStatsRepository } from './repositories/RollStatsRepository';
import { ChannelCommandRepository } from './repositories/ChannelCommandRepository';
import { EnvUtil } from '../modules/util/EnvUtil';

const postgresHost = EnvUtil.getRequiredEnvVariable('TaylorBot_DatabaseConnection__Host');
const postgresPort = EnvUtil.getRequiredEnvVariable('TaylorBot_DatabaseConnection__Port');
const postgresSSL = EnvUtil.getRequiredEnvVariable('TaylorBot_DatabaseConnection__UseSSL');
const postgresMaxPoolSize = EnvUtil.getRequiredEnvVariable('TaylorBot_DatabaseConnection__MaxPoolSize');
const postgresUsername = EnvUtil.getRequiredEnvVariable('TaylorBot_DatabaseConnection__Username');
const postgresPassword = EnvUtil.getRequiredEnvVariable('TaylorBot_DatabaseConnection__Password');

export class DatabaseDriver {
    readonly guilds: GuildRepository;
    readonly users: UserRepository;
    readonly guildMembers: GuildMemberRepository;
    readonly usernames: UsernameRepository;
    readonly guildNames: GuildNameRepository;
    readonly guildCommands: GuildCommandRepository;
    readonly commands: CommandRepository;
    readonly userGroups: UserGroupRepository;
    readonly roleGroups: RoleGroupRepository;
    readonly textChannels: TextChannelRepository;
    readonly attributes: AttributeRepository;
    readonly textAttributes: TextAttributeRepository;
    readonly rollStats: RollStatsRepository;
    readonly channelCommands: ChannelCommandRepository;

    constructor() {
        const db = pgp({
            host: postgresHost,
            port: Number.parseInt(postgresPort),
            database: PostgreSQLConfig.database,
            user: postgresUsername,
            password: postgresPassword,
            ssl: postgresSSL === 'true' ? { rejectUnauthorized: false } : false,
            max: Number.parseInt(postgresMaxPoolSize),
            application_name: 'commands-attributes'
        });
        const helpers = pgp.helpers;

        const usersDAO = new UserDAO();

        this.guilds = new GuildRepository(db);
        this.users = new UserRepository(db);
        this.guildMembers = new GuildMemberRepository(db, helpers);
        this.usernames = new UsernameRepository(db);
        this.guildNames = new GuildNameRepository(db);
        this.guildCommands = new GuildCommandRepository(db);
        this.commands = new CommandRepository(db);
        this.userGroups = new UserGroupRepository(db, helpers);
        this.roleGroups = new RoleGroupRepository(db);
        this.textChannels = new TextChannelRepository(db);
        this.attributes = new AttributeRepository(db, helpers);
        this.textAttributes = new TextAttributeRepository(db);
        this.rollStats = new RollStatsRepository(db, usersDAO);
        this.channelCommands = new ChannelCommandRepository(db);
    }
}
