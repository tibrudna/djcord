using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tibrudna.djcort.src.Dao;
using tibrudna.djcort.src.Models;

namespace tibrudna.djcort.src.Services
{
    /// <summary>Holds a queue for the songs that will be played.</summary>
    public class PlaylistService
    {
        private readonly Queue<Song> playlist;
        private readonly DatabaseContext databaseContext;

        /// <summary>The Contructor for the service.</summary>
        /// <param name="databaseContext">The connector to the database.</param>
        public PlaylistService(DatabaseContext databaseContext)
        {
            playlist = new Queue<Song>();
            this.databaseContext = databaseContext;
        }

        /// <summary>Takes one song out of the playlist queue.</summary>
        /// <returns>The first song in the queue.</returns>
        public Song Dequeue() => playlist.Dequeue();

        /// <summary>Adds a song to the end of the playlist.</summary>
        /// <param name="song">The song to insert into the database.</param>
        public void Enqueue(Song song) => playlist.Enqueue(song);

        /// <summary>The number of songs waiting in the queue.</summary>
        /// <returns>The number of songs in the queue.</returns>
        public int Count { get { return playlist.Count; }}

        /// <summary>Loads all the songs from the database in to the playlist.</summary>
        /// <returns>A task representing this method.</returns>
        public Task LoadPlaylist()
        {
            var songs = databaseContext.songs.ToArray<Song>();
            Shuffle(songs);
            songs.ToList().ForEach(s => playlist.Enqueue(s));
            return Task.CompletedTask;
        }

        private void Shuffle(Song[] songs)
        {
            var random = new Random();
            for (int i = songs.Length -1; i > 0; i--)
            {
                var j = random.Next(i+1);
                var song = songs[j];
                songs[j] = songs[i];
                songs[i] = song;
            }
        }

    }
}