using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using tibrudna.djcort.src.Audio;
using tibrudna.djcort.src.Database;

namespace tibrudna.djcort.src.Commands
{
    ///<summary>Commands for interacting with the database.</summary>
    [Group("db")]
    public class DatabaseCommand : ModuleBase<SocketCommandContext>
    {
        private readonly DatabaseContext database;
        private readonly AudioManager audioManager;

        ///<summary>Creates a new instance of a DatabaseCommand.</summary>
        ///<param name="database">The DatabaseContext containing the data.</param>
        ///<param name="audioManager">AudioManager for handling the createion of songs.</param>
        public DatabaseCommand(DatabaseContext database, AudioManager audioManager)
        {
            this.database = database;
            this.audioManager = audioManager;
        }

        ///<summary>Command for adding songs to the database.</summary>
        ///<param name="url">Url for the song to be added.</param>
        [Command("add")]
        public async Task Add(string url)
        {
            var song = await audioManager.LoadSongAsync(url);
            try
            {
                await database.Songs.AddAsync(song);
                await database.SaveChangesAsync();
                await ReplyAsync("Song was added to the database.");
            }
            catch (Exception)
            {
                await ReplyAsync("Song could not be added to the database.");
            }
        }

        ///<summary>Command for removing songs from the database.</summary>
        ///<param name="id">The id for the song to be removed.</param>
        [Command("rm")]
        public async Task Remove(string id)
        {
            var song = await database.Songs.SingleOrDefaultAsync(s => s.Id.Equals(id));
            if (song == null) {
                await ReplyAsync("There is no song in the database with this ID.");
                return;
            }

            try
            {
                database.Songs.Remove(song);
                await database.SaveChangesAsync();
                await ReplyAsync("Song was removed from the database.");
            }
            catch (Exception)
            {
                await ReplyAsync("Song could not be removed from the database.");
            }
        }

        ///<summary>The command for receiving all songs in the database.</summary>
        [Command("all")]
        public async Task GetAll()
        {
            var songs = database.Songs.ToList();

            if (songs.Count == 0)
            {
                await ReplyAsync("There are no songs in the playlist.");
                return;
            }

            var response = "";
            foreach (var song in songs)
            {
                response += $"{song.Id} - {song.Title}\n";
            }

            await ReplyAsync(response);
        }

        ///<summary>The command for finding a song by its name.</summary>
        ///<param name="name">The name of the song to look for.</param>
        [Command("name")]
        public async Task FindSongByTitle(string name)
        {
            var songs = database.Songs.AsQueryable().Where(s => s.Title.Contains(name)).ToList();

            if (songs.Count == 0)
            {
                await ReplyAsync("No song was found.");
                return;
            }

            var response = "";
            foreach (var song in songs)
            {
                response += $"{song.Id} - {song.Title}\n";
            }

            await ReplyAsync(response);
        }
    }
}