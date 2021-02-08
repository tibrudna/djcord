import * as discord from 'discord.js';
import { AudioManager } from './audio/audiomanager';
import { CommandService } from './command/commandservice';
import * as audio from './command/audiocommand';

const client = new discord.Client();
const service = new CommandService();
const audioManager = new AudioManager();

service.add("ping", msg => {
    msg.channel.send("pong");
});

service.add("join", async msg => audio.join(msg, audioManager));
service.add("add", msg => audio.add(msg, audioManager));
service.add("play", msg => audio.play(msg, audioManager));

client.on('ready', () => {
    console.log('Logged in');
});

client.on('message', async msg => {
    if (msg.author.bot) return;

    if (!msg.content.startsWith("!")) return;

    let content = msg.content.replace("!", "").split(" ", 1);
    service.get(content[0])(msg);

});

function exitHandler(options, exitCode) {
    console.log("Closed connection");
    client.destroy();
    if (options.exit) process.exit();
}

process.on("SIGINT", exitHandler.bind(null, {exit: true}));

client.login(process.env.TOKEN);