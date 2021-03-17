import * as discord from 'discord.js';
import { AudioPlayer } from './audioplayer';
import { Logger } from '../helper/Logger';

class AudioManager {
    register: Map<string, AudioPlayer>;

    constructor() {
        this.register = new Map();
    }

    async registerNewAudio(channel: discord.VoiceChannel) {
        let guildID = channel.guild.id;
        let connection = await channel.join();
        let audioPlayer = new AudioPlayer(connection);

        this.register.set(guildID, audioPlayer);
        Logger.info("New player was registered");
    }

    addSong(guildID: string, song: string) {
        let player = this.register.get(guildID);
        player.enqueu(song);
    }

    startPlayer(guildID: string) {
        let player = this.register.get(guildID);
        player.play();
    }
}

export { AudioManager };