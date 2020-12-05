using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using VideoLibrary;

namespace tibrudna.djcort.src.Audio
{
    /// <summary>
    /// This class is responsible for managing every instance of AudioPlayer.
    /// </summary>
    public class AudioManager
    {
        public AudioPlayer AudioPlayer { get; private set; }

        /// <summary>
        /// Creates a new audioplayer.
        /// </summary>
        /// <returns>A task, that represents the creation of a new instance of an AudioPlayer.</returns>
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
        public async Task<Video> LoadSongAsync(string url)
        {
            var ytdl = YouTube.Default;
            var video = await ytdl.GetVideoAsync(url);
            return video;
        }
    }
}