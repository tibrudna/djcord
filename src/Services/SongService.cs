using System.Threading.Tasks;
using System.Linq;
using tibrudna.djcort.src.Dao;
using tibrudna.djcort.src.Models;
using System.Text.RegularExpressions;
using VideoLibrary;
using System;
using tibrudna.djcort.src.Exceptions;
using System.Collections.Generic;

namespace tibrudna.djcort.src.Services
{
    /// <summary>
    /// This class provides services for the handling of songs.
    /// It represents the layer between the database and the songs module.
    /// </summary>
    public class SongService
    {
        private readonly DatabaseContext database;

        /// <summary>Contructor for the SongService</summary>
        /// <param name="database">The DatabaseContext for this service.</param>
        public SongService(DatabaseContext database)
        {
            this.database = database;
        }

        /// <summary>Adds a song, which is not in the database, into the database.</summary>
        /// <param name="song">The song to add to the database.</param>
        /// <exception cref="System.ArgumentException">Thrown, when the song is not valid</exception>
        /// <exception cref="tibrudna.djcort.src.Exceptions.DuplicateSongException">Thrown, when the song already exists in the database</exception>
        public void Add(Song song)
        {
            if (song == null) throw new ArgumentException("song");
            if (Exists(song.ID)) throw new DuplicateSongException();

            database.songs.Add(song);
            database.SaveChanges();
        }

        /// <summary>Removes a song from the database.</summary>
        /// <param name="song">The song to remove from the database.</param>
        public void RemoveSong(Song song)
        {
            if (song == null) throw new ArgumentException("song");
            if (!Exists(song.ID)) throw new SongNotInDatabaseException();

            database.songs.Remove(song);
            database.SaveChanges();
        }

        /// <summary>Checks if a song is already in the database.</summary>
        /// <param name="id">The song id to check for existence.</param>
        /// <returns>Wether a song with the id is in the database.</returns>
        /// <exception cref="System.ArgumentException">Thrown, when the song is not valid</exception>
        public bool Exists(string id)
        {
            if (id == null || id.Equals("")) throw new ArgumentException("id");
            return database.songs.Any<Song>(s => s.ID.Equals(id));
        }

        /// <summary>Look for a song with the given id.</summary>
        /// <param name="id">The id of the song to look for.</param>
        /// <returns>The song found or null if no song with the given id was found.</returns>
        /// <exception cref="System.ArgumentException">Thrown, when the id is null or empty.</exception>
        public Song FindSongByID(string id)
        {
            if (id == null || id.Equals("")) throw new ArgumentException("id");
            return database.songs.SingleOrDefault<Song>(s => s.ID.Equals(id));
        }

        /// <summary>Get all Songs with the given string in the title.</summary>
        /// <param name="title">The string to look for.</param>
        /// <returns>A list with all the songs, that cotains the given string in the title.</returns>
        /// <exception cref="System.ArgumentException">Thrown, when the title is null or empty.</exception>
        public List<Song> FindSongByTitle(string title)
        {
            if (title == null || title.Equals("")) throw new ArgumentException("title");
            title = title.ToLower();
            return Queryable
                    .Where<Song>(database.songs, s => s.Title.ToLower().Contains(title) || s.Artist.ToLower().Contains(title))
                    .ToList<Song>();
        }

        /// <summary>Get all songs in the database.</summary>
        /// <returns>A list of all songs in the database.</returns>
        public List<Song> GetAll()
        {
            return database.songs.ToList();
        }

        /// <summary>Gets the stream url for a video.</summary>
        /// <param name="song">The song for which the stream url should be received.</param>
        /// <returns>A task which returns the url for the stream.</returns>
        public async Task<string> GetStreamUrl(Song song)
        {
            var youtube = YouTube.Default;
            var video = await youtube.GetVideoAsync(song.Url);
            return await video.GetUriAsync();
        }


        /// <summary>Creates a new valid Song.</summary>
        /// <param name="url">The url to the video for which the song should be created.</param>
        /// <returns>A task which returns the new create Song.</returns>
        public async Task<Song> CreateNewSong(string url)
        {
            Song song = new Song();
            var youtube = YouTube.Default;
            var video = await youtube.GetVideoAsync(url);

            if (video.Title.Contains('-'))
            {
                var titleParts = video.Title.Split("-");
                song.Artist = titleParts[0];
                song.Title = titleParts[1];
            }
            else
            {
                song.Title = video.Title;
            }

            song.ID = ParseID(url);

            return song;
        }

        /// <summary>Cuts the id out from the url.</summary>
        /// <param>The url which contains the id.</param>
        /// <returns>The id which was in the url.</returns>
        public string ParseID(string url)
        {
            var pattern = "v=[\\w-]*";
            var match = Regex.Match(url, pattern);
            return match.Value.Remove(0, 2);
        }

    }
}