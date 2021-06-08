using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Djcord.Bot.Audio
{

    public class AudioService : IAudioService
    {
        private readonly LinkedList<string> _trackSchedule;
        private CancellationTokenSource songTokenSource;

        public AudioService()
        {
            _trackSchedule = new LinkedList<string>();
        }

        public void Add(string url)
        {
            _trackSchedule.AddLast(url);
        }

        public void Next()
        {
            songTokenSource.Cancel();
        }

        public async Task PlayAsync(SocketCommandContext context)
        {
            var channel = (context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) await context.Channel.SendMessageAsync("User is not in a voice channel");

            var audioClient = await channel.ConnectAsync();

            while (_trackSchedule.Count > 0)
            {
                songTokenSource = new CancellationTokenSource();
                var nextSong = _trackSchedule.First;
                _trackSchedule.RemoveFirst();

                var youtube = new YoutubeClient();
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(nextSong.Value);
                var stream = streamManifest.GetMuxedStreams().GetWithHighestBitrate().Url;

                await SendAsync(audioClient, stream);

            }
        }

        private async Task SendAsync(IAudioClient client, string path)
        {
            var streamToken = songTokenSource.Token;
            using (var ffmpeg = CreateStream(path))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try
                {
                    var buffer = new byte[1024];
                    int readBytes;

                    while ((readBytes = await output.ReadAsync(buffer)) > 0)
                    {
                        if (streamToken.IsCancellationRequested) break;
                        await discord.WriteAsync(buffer);
                    }
                }
                finally
                {
                    await discord.FlushAsync();
                    songTokenSource.Dispose();
                }
            }
        }

        private Process CreateStream(string url)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{url}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }
    }
}