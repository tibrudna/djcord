using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;
using VideoLibrary;

namespace tibrudna.djcort.src.Audio
{
    public class AudioSendHandler
    {
        /// <summary> Represents a task, that sends audio through a channel.</summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="cancelToken"></param>
        public async Task SendAsync(IAudioClient client, string url, CancellationToken cancelToken)
        {
            using (var ffmpeg = CreateStream(url))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var stream = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try
                {
                    byte[] buffer = new byte[1024];

                    int i = 0;
                    while ((i = output.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (cancelToken.IsCancellationRequested) break;
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                }
                finally
                {
                    await stream.FlushAsync();
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