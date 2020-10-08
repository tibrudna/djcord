using System.Collections.Generic;
using tibrudna.djcort.src.Models;

namespace tibrudna.djcort.src.Services
{
    public class PlaylistService
    {
        private readonly Queue<Song> playlist;

        public PlaylistService()
        {
            playlist = new Queue<Song>();
        }

        public Song Dequeue() => playlist.Dequeue();

        public void Enqueue(Song song) => playlist.Enqueue(song);

        public int Count { get { return playlist.Count; }}

    }
}