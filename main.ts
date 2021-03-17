import * as discord from 'discord.js';
import { AudioManager } from './audio/audiomanager';
import { CommandService } from './command/commandservice';
import { Logger } from './helper/Logger';
import * as audio from './command/audiocommand';

Logger.level = 'info';
const client = new discord.Client();
const service = new CommandService();
const audioManager = new AudioManager();

service.add("join", async msg => audio.join(msg, audioManager));
service.add("add", msg => audio.add(msg, audioManager));
service.add("play", msg => audio.play(msg, audioManager));

client.on('ready', () => {
    Logger.info("Bot is ready to go")
});

client.on('message', async msg => {
    if (msg.author.bot) return;

    if (!msg.content.startsWith("!")) return;
    msg.content = msg.content.replace("!", "");

    let command = msg.content.split(" ", 1)[0];
    let func = service.get(command);
    
    msg.content = msg.content.replace(command, "");
    func(msg);
});

function exitHandler(options, exitCode) {
    client.destroy();
    Logger.info("Bot closed connection")
    if (options.exit) process.exit();
}

process.on("SIGINT", exitHandler.bind(null, {exit: true}));

client.login(process.env.TOKEN)
        .catch(err => Logger.error("No connection with this Token"));