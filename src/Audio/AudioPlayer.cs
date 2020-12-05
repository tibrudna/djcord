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
    /// <summary>This class is responsible for playing songs.</summary>
    public class AudioPlayer
    {
        private Queue<Video> trackScheduler;
        private IAudioClient audioClient;
        private CancellationTokenSource tokenSource;

        ///<summary>Creates a new instance of an Audioplayer.</summary>
        ///<param name="audioClient">Is the client, that holds the connection to a channel.</param>
        ///<returns>A new Instance of AudioPlayer.</returns>
        public AudioPlayer(IAudioClient audioClient)
        {
            this.audioClient = audioClient;

            trackScheduler = new Queue<Video>();

        }

        /// <summary>Starts the AudioPlayer to play music.</summary>
        /// <returns>A task representing the action of playing music.</returns>
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

        ///<summary>Skip the currently playing song.</summary>
        ///<returns>A task, representing the action of skipping a song.</returns>
        public Task Next()
        {
            tokenSource.Cancel();
            return Task.CompletedTask;
        }

        ///<summary>Stops the player from playing songs.</summary>
        public void Stop()
        {

        }

        ///<summary>Remove all songs from the playlist.</summary>
        public void Flush()
        {

        }

        ///<summary>Adds a new Song to the playlist.</summary>
        ///<param name="song">The songs, that is added to the playlist.</param>
        public void Enqueue(Video song)
        {
            trackScheduler.Enqueue(song);
        }
    }
}