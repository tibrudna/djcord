using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using VideoLibrary;

namespace tibrudna.djcort.src.Audio
{
    public class AudioPlayer
    {
        private Queue<Video> trackScheduler;
        private IAudioClient audioClient;
        private CancellationTokenSource tokenSource;

        public AudioPlayer(IAudioClient audioClient)
        {
            this.audioClient = audioClient;

            trackScheduler = new Queue<Video>();

        }

        public async Task Start()
        {
            while (trackScheduler.Count > 0)
            {
                var nextTrack = trackScheduler.Dequeue();
                var audioSendHandler = new AudioSendHandler();

                tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                Debug.WriteLine(nextTrack.Uri);

                await audioSendHandler.SendAsync(audioClient, nextTrack.Uri, token);
                tokenSource.Dispose();
            }
        }

        public Task Next()
        {
            tokenSource.Cancel();
            return Task.CompletedTask;
        }

        public void Stop()
        {

        }

        public void Flush()
        {

        }

        public void Enqueue(Video song)
        {
            trackScheduler.Enqueue(song);
        }
    }
}