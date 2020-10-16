using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using tibrudna.djcort.src.Exceptions;
using tibrudna.djcort.src.Models;
using VideoLibrary;
using tibrudna.djcort.src.Dao;
using System;

namespace tibrudna.djcort.src.Services
{
    /// <summary>Controlls the music player</summary>
    public class PlayerService
    {
        private readonly PlaylistService playlist;
        private readonly SongService songService;
        private IAudioClient audioClient;
        private Song currentSong;
        private bool nextSong;
        private Task playStatus;

        /// <summary>Creates a new instance of the PlayerService class.</summary>
        /// <param name="playlist">Service which controlls the playlist.</param>
        /// <param name="songService">Service which operates with songs.</param>
        public PlayerService(PlaylistService playlist, SongService songService)
        {
            this.playlist = playlist;
            this.songService = songService;
            nextSong = false;
            playStatus = Task.CompletedTask;
        }

        /// <summary>Joins a users voice channel.</summary>
        /// <param name="user">The user the music player should connect to.</param>
        public async Task JoinChannel(SocketUser user)
        {
            var channel = (user as IGuildUser)?.VoiceChannel;
            if (channel == null) throw new UserNotInVoiceChannelException("The user is in no voice channel");

            audioClient = await channel.ConnectAsync();
        }

        /// <summary>Skip the current song.</summary>
        public void NextSong()
        {
            nextSong = true;
        }

        /// <summary>Starts the player.</summary>
        public void Start()
        {
            playStatus = StartPlaying();
        }

        private async Task StartPlaying()
        {
            byte[] buffer = new byte[1024];
            while(playlist.Count > 0)
            {
                currentSong = playlist.Dequeue();
                var streamUrl = await songService.GetStreamUrl(currentSong);
                using (var ffmpeg = CreateStream(streamUrl))
                using (var output = ffmpeg.StandardOutput.BaseStream)
                using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed))
                {
                    try
                    {
                        int read = -1;
                        while((read = await output.ReadAsync(buffer, 0, buffer.Length)) > 0 && !nextSong)
                        {
                            await discord.WriteAsync(buffer, 0, buffer.Length);
                        }
                    }
                    finally
                    {
                        await discord.FlushAsync();
                        nextSong = false;
                    }
                }
            }
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        /// <summary>Informations about the current song as an embed.</summary>
        /// <returns>An Embed containing the Information about the song.</returns>
        public Embed NowPlaying()
        {
            var builder = new EmbedBuilder();
            return builder.WithColor(Color.Blue)
                        .WithTitle(currentSong.Title)
                        .WithDescription($"by {currentSong.Artist}")
                        .WithUrl(currentSong.Url)
                        .WithThumbnailUrl(currentSong.ThumbnailUrl)
                        .Build();
        }

    }
}