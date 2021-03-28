import * as discord from 'discord.js'
import { AudioManager } from '../audio/audiomanager';

function join(msg: discord.Message, audioManager: AudioManager): void {
    let channel = msg.member.voice.channel;
    audioManager.registerNewAudio(channel);
}

function add(msg: discord.Message, audioManager: AudioManager): void {
    let song = msg.content.trim();
    let guildID = msg.guild.id;

    audioManager.addSong(guildID, song);
    msg.channel.send("Song was added");
}

function play(msg: discord.Message, audioManager: AudioManager): void {
    audioManager.startPlayer(msg.guild.id);
}

export { join, add, play }