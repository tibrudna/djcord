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

        public PlayerService()
        {
            playlist = new Queue<string>();
            tempFiles = new Queue<string>();
            loadStatus = Task.CompletedTask;
        }

        public async Task AddToPlaylist(SocketCommandContext context, string url)
        {
            playlist.Enqueue(url);
            await context.Channel.SendMessageAsync("Song added");
            if (playlist.Count < 2) loadStatus = LoadSong();
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

        private async Task LoadSong()
        {
            var tempFile = Path.GetTempFileName();
            var youtube = YouTube.Default;
            if (playlist.Count < 1) return;

            var video = await youtube.GetVideoAsync(playlist.Dequeue());
            await File.WriteAllBytesAsync(tempFile, await video.GetBytesAsync());
            tempFiles.Enqueue(tempFile);
        }

        public async Task StartPlaying()
        {
            await loadStatus;
            while(tempFiles.Count > 0) {
                loadStatus = LoadSong();
                var tempFile = tempFiles.Dequeue();
                using (var ffmpeg = CreateStream(tempFile))
                using (var output = ffmpeg.StandardOutput.BaseStream)
                using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed))
                {
                    try { await output.CopyToAsync(discord); }
                    finally { await discord.FlushAsync(); }
                }
                File.Delete(tempFile);
                await loadStatus;
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