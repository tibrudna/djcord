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
        private readonly Queue<string> playlist;
        private IAudioClient audioClient;
        private Song currentSong;
        private bool nextSong;

        public PlayerService()
        {
            playlist = new Queue<string>();
            nextSong = false;
        }

        public async Task JoinChannel(SocketUser user)
        {
            var channel = (user as IGuildUser)?.VoiceChannel;
            if (channel == null) throw new UserNotInVoiceChannelException("The user is in no voice channel");

            audioClient = await channel.ConnectAsync();
        }

        public void AddToPlaylist(string url)
        {
            playlist.Enqueue(url);
        }

        public void NextSong()
        {
            nextSong = true;
        }

        public async Task StartPlaying()
        {
            var youtube = YouTube.Default;
            byte[] buffer = new byte[1024];
            while(playlist.Count > 0)
            {
                var currentUrl = playlist.Dequeue();
                currentSong = new Song(currentUrl, await youtube.GetVideoAsync(currentUrl));
                using (var ffmpeg = CreateStream(await currentSong.Video.GetUriAsync()))
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
            var songParts = TitleParser(currentSong.Video.Title);
            return builder.WithColor(Color.Blue)
                        .WithTitle(currentSong.Title)
                        .WithDescription($"by {currentSong.Artist}")
                        .WithUrl(currentSong.Url)
                        .WithThumbnailUrl(currentSong.ThumbnailUrl)
                        .Build();
        }

        private string[] TitleParser(string title)
        {
            var parts = title.Split('-', 2);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }

            return parts;
        }

    }
}