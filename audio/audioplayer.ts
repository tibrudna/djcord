import * as discord from 'discord.js'
import * as ytdl from 'ytdl-core';

class AudioPlayer {
    playlist: string[];
    connection: discord.VoiceConnection;

    constructor(connection: discord.VoiceConnection) {
        this.connection = connection;
        this.playlist = [];
    }

    enqueu(song: string) {
        this.playlist.push(song);
    }

    dequeue(): string {
        return this.playlist.shift();
    }

    play() {
        if (this.playlist.length <= 0) {
            return;
        }

        let nextSong = this.playlist.shift();
        let dispatcher = this.connection.play(ytdl(nextSong));
        dispatcher.on("finish", () => {
            this.play();
        });
    }
}

export { AudioPlayer };