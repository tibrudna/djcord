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
        private IAudioClient audioClient;

        public PlayerService()
        {
            playlist = new Queue<string>();
        }

        public async Task AddToPlaylist(string url) => playlist.Enqueue(url);

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
            while(playlist.Count > 0) {
                var video = await youtube.GetVideoAsync(playlist.Dequeue());
                var tempFile = Path.GetTempFileName();
                await File.WriteAllBytesAsync(tempFile, await video.GetBytesAsync());

                using (var ffmpeg = CreateStream(tempFile))
                using (var output = ffmpeg.StandardOutput.BaseStream)
                using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed))
                {
                    try { await output.CopyToAsync(discord); }
                    finally { await discord.FlushAsync(); }
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