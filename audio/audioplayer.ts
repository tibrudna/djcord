import * as discord from 'discord.js'
import * as ytdl from 'ytdl-core';
import { LinkedList } from '../helper/LinkedList';

class AudioPlayer {
    playlist: LinkedList<string>;
    connection: discord.VoiceConnection;

    constructor(connection: discord.VoiceConnection) {
        this.connection = connection;
        this.playlist = new LinkedList<string>();
    }

    enqueu(song: string) {
        this.playlist.push(song);
    }

    dequeue(): string {
        return this.playlist.pop();
    }

    play() {
        if (this.playlist.length() == 0) {
            return;
        }

        let nextSong = this.playlist.pop();
        let dispatcher = this.connection.play(ytdl(nextSong));
        dispatcher.on("finish", () => {
            this.play();
        });
    }
}

export { AudioPlayer };