using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using VideoLibrary;

namespace tibrudna.djcort.src.Services
{
    public class PlayerService
    {
        private readonly Queue<string> playlist;
        private readonly Queue<string> tempFiles;
        private IAudioClient audioClient;
        private Task loadStatus;
        private bool nextSong;

        public PlayerService()
        {
            playlist = new Queue<string>();
            tempFiles = new Queue<string>();
            loadStatus = Task.CompletedTask;
            nextSong = false;
        }

        public async Task NextSong()
        {
            nextSong = true;
        }

        public async Task AddToPlaylist(SocketCommandContext context, string url)
        {
            playlist.Enqueue(url);
            await context.Channel.SendMessageAsync("Song added");
        }

        public async Task JoinChannel(SocketCommandContext context)
        {
            var channel = (context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) {
                await context.Channel.SendMessageAsync("User must be in a voice channel");
                return;
            }

            audioClient = await channel.ConnectAsync();
        }

        public async Task StartPlaying()
        {
            var youtube = YouTube.Default;
            byte[] buffer = new byte[1024];
            while(playlist.Count > 0)
            {
                var video = await youtube.GetVideoAsync(playlist.Dequeue());
                using (var ffmpeg = CreateStream(await video.GetUriAsync()))
                using (var output = ffmpeg.StandardOutput.BaseStream)
                using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed))
                {
                    try
                    {
                        int read = -1;
                        while((read = await output.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            if (nextSong) continue;
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
    }
}