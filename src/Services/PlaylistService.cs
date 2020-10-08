using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tibrudna.djcort.src.Dao;
using tibrudna.djcort.src.Models;

namespace tibrudna.djcort.src.Services
{
    public class PlaylistService
    {
        private readonly Queue<Song> playlist;
        private readonly DatabaseContext databaseContext;

        public PlaylistService(DatabaseContext databaseContext)
        {
            playlist = new Queue<Song>();
            this.databaseContext = databaseContext;
        }

        public Song Dequeue() => playlist.Dequeue();

        public void Enqueue(Song song) => playlist.Enqueue(song);

        public int Count { get { return playlist.Count; }}

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