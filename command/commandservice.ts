import * as discord from 'discord.js';

interface Command {
    (message: discord.Message): void;
}

class CommandService {
    register: Map<string, Command>;

    constructor() {
        this.register = new Map();
    }

    add(name: string, command: Command): void {
        this.register.set(name, command);
    }

    get(name: string): Command {
        return this.register.get(name);
    }
}

export { CommandService };