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

namespace tibrudna.djcort.src.Services
{
    public class PlayerService
    {
        private readonly Queue<Song> playlist;
        private IAudioClient audioClient;
        private Song currentSong;
        private bool nextSong;
        private Task playStatus;

        public PlayerService()
        {
            playlist = new Queue<Song>();
            nextSong = false;
            playStatus = Task.CompletedTask;
        }

        public async Task JoinChannel(SocketUser user)
        {
            var channel = (user as IGuildUser)?.VoiceChannel;
            if (channel == null) throw new UserNotInVoiceChannelException("The user is in no voice channel");

            audioClient = await channel.ConnectAsync();
        }

        public async Task AddToPlaylist(string url)
        {
            var youtube = YouTube.Default;
            var video = await youtube.GetVideoAsync(url);

            playlist.Enqueue(Song.NewSong(url, video));

            if (!playStatus.IsCompleted) return;
            playStatus = StartPlaying();
        }

        public void NextSong()
        {
            nextSong = true;
        }

        public async Task StartPlaying()
        {
            byte[] buffer = new byte[1024];
            while(playlist.Count > 0)
            {
                currentSong = playlist.Dequeue();
                using (var ffmpeg = CreateStream(currentSong.StreamUrl))
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