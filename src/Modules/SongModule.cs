using System;
using System.Threading.Tasks;
using Discord.Commands;
using tibrudna.djcort.src.Models;
using tibrudna.djcort.src.Services;
using tibrudna.djcort.src.Exceptions;
using System.Text.RegularExpressions;

namespace tibrudna.djcort.src.Modules
{
    /// <summary>Contains comands to controll songs.</summary>
    [Group("db")]
    public class SongModule : ModuleBase<SocketCommandContext>
    {
        private readonly SongService songService;

        /// <summary>Creates a new instance of the SongModule class.</summary>
        /// <param name="songService">The service that contains controlls for songs.</param>
        public SongModule(SongService songService)
        {
            this.songService = songService;
        }

        /// <summary>Deletes a song from the database.</summary>
        /// <param name="id">The song id.</param>
        /// <returns>This method as a task.</returns>
        [Command("rm")]
        public async Task RemoveSong(string id)
        {
            var song = songService.FindSongByID(id);
            songService.RemoveSong(song);
            await Context.Channel.SendMessageAsync("Song was removed.");
        }

        /// <summary>Adds a song to the database.</summary>
        /// <param name="url">The url to the song.</param>
        /// <returns>This method as a task.</returns>
        [Command("add")]
        public async Task AddToDb(string url)
        {
            if (!ValidateUrl(url))
            {
                await Context.Channel.SendMessageAsync("Bad url syntax.");
                return;
            }
            var id = songService.ParseID(url);
            if (songService.Exists(id))
            {
                await Context.Channel.SendMessageAsync("Song is already in the database");
                return;
            }

            var song = await songService.CreateNewSong(url);
            songService.Add(song);
            await Context.Channel.SendMessageAsync("Song was added to the database");
        }

        private bool ValidateUrl(string url)
        {
            return Regex.IsMatch(url, "https:\\/\\/www\\.youtube\\.com\\/watch\\?v=[\\w-]*");
        }

        /// <summary>Find all songs containing the string in the title.</summary>
        /// <param name="title">Part of the title to look for.</param>
        /// <returns>This method as a task.</returns>
        [Command("fname")]
        public async Task FindSongByTitle(string title)
        {
            var response = "";
            var songs = songService.FindSongByTitle(title);
            if (songs.Count == 0) response += $"No song with {title} was found.";
            else
            {
                foreach (var song in songs)
                {
                    response += song.ToString() + "\n";
                }
            }

            await Context.Channel.SendMessageAsync(response);
        }

        /// <summary>Finds a song by its id.</summary>
        /// <param name="id">The song id.</param>
        /// <returns>This method as a task.</returns>
        [Command("fid")]
        public async Task FindSongByID(string id)
        {
            var song = songService.FindSongByID(id);
            var response = (song == null) ? $"Couldn't find {id}." : song.ToString();

            await Context.Channel.SendMessageAsync(response);
        }

        /// <summary>Get all songs in the database.</summary>
        /// <returns>This method as a task.</returns>
        [Command("all")]
        public async Task GetAllSongs()
        {
            var songs = songService.GetAll();
            var response = "";
            foreach (var song in songs)
            {
                response += song.ToString() + "\n";
            }

            await Context.Channel.SendMessageAsync(response);
        }
    }
}