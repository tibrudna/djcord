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
        private CancellationTokenSource songTokenSource;
        private CancellationTokenSource playerTokenSource;

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
            playerTokenSource = new CancellationTokenSource();
            var playerToken = playerTokenSource.Token;

            while (trackScheduler.Count > 0)
            {
                if (playerToken.IsCancellationRequested) {
                    break;
                }

                var nextTrack = trackScheduler.Dequeue();
                var audioSendHandler = new AudioSendHandler();

                songTokenSource = new CancellationTokenSource();
                var token = songTokenSource.Token;
                Debug.WriteLine(nextTrack.Uri);

                await audioSendHandler.SendAsync(audioClient, nextTrack.Uri, token);
                songTokenSource.Dispose();
            }

            playerTokenSource.Dispose();
        }

        ///<summary>Skip the currently playing song.</summary>
        ///<returns>A task, representing the action of skipping a song.</returns>
        public Task Next()
        {
            if (songTokenSource == null) return Task.CompletedTask;

            songTokenSource.Cancel();
            return Task.CompletedTask;
        }

        ///<summary>Stops the player from playing songs.</summary>
        ///<returns>A task, representing the action of stopping the player.</returns>
        public Task Stop()
        {
            if (playerTokenSource == null) return Task.CompletedTask;

            playerTokenSource.Cancel();
            this.Next();

            return Task.CompletedTask;
        }

        ///<summary>Remove all songs from the playlist.</summary>
        public void ClearPlaylist()
        {
            trackScheduler.Clear();
        }

        ///<summary>Adds a new Song to the playlist.</summary>
        ///<param name="song">The songs, that is added to the playlist.</param>
        public void Enqueue(Video song)
        {
            trackScheduler.Enqueue(song);
        }
    }
}