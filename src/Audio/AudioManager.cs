using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using tibrudna.djcort.src.Models;
using VideoLibrary;

namespace tibrudna.djcort.src.Audio
{
    /// <summary>
    /// This class is responsible for managing every instance of AudioPlayer.
    /// </summary>
    public class AudioManager
    {
        ///<summary>The audioplayer responsible for playing music in the guild.</summary>
        public AudioPlayer AudioPlayer { get; private set; }

        ///<summary>Creates a new audioplayer.</summary>
        ///<param name="channel">The channel to connect to.</param>
        ///<returns>A task, that represents the creation of a new instance of an AudioPlayer.</returns>
        public async Task CreateNewAudioPlayer(IAudioChannel channel)
        {
            var connection = await channel.ConnectAsync();
            AudioPlayer = new AudioPlayer(connection);
        }


        /// <summary>This Method will load the information for a song.</summary>
        /// <param name="url">The path to the song.</param>
        /// <returns>
        /// A Task that represents the information loading for a song. The result contains the information.
        /// </returns>
        public async Task<Song> LoadSongAsync(string url)
        {
            var videoTask = LoadVideoAsync(url);
            var idTask = GetIdFromUrlAsync(url);

            var video = await videoTask;
            var id = await idTask;

            return new Song(id, video.Title, video.Uri);
        }

        private async Task<Video> LoadVideoAsync(string url)
        {
            var ytdl = YouTube.Default;
            var video = await ytdl.GetVideoAsync(url);
            return video;
        }

        private Task<string> GetIdFromUrlAsync(string url)
        {
            string pattern = "(=|\\/){1}[\\w-]+$";
            var regex = new Regex(pattern);

            var match = regex.Match(url);
            if (!match.Success) return null;

            return Task.FromResult(match.Value.Remove(0,1));
        }

        private void onAudioPlayerDisconnect()
        {
        }
    }
}