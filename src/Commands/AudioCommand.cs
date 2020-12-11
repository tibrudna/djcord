using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using tibrudna.djcort.src.Audio;

namespace tibrudna.djcort.src.Commands
{
    ///<summary>This class is responsible for handling commands, regarding the audio.</summary>
    public class AudioCommand : ModuleBase<SocketCommandContext>
    {
        private readonly AudioManager audioManager;

        ///<summary>Creates a new instance of the AudioCommand.</summary>
        ///<param name="audioManager">The AudioManager reponsible for managing the audio.</param>
        ///<returns>A new instance of the AudioCommand.</returns>
        public AudioCommand(AudioManager audioManager)
        {
            this.audioManager = audioManager;
        }

        ///<summary>Command to join a voice channel.</summary>
        ///<returns>A task, representing the action of joining the user voice channel.</returns>
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinVoiceChannel()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) {
                await ReplyAsync("User is not in a voice channel!");
            }

            await audioManager.CreateNewAudioPlayer(channel);
        }

        ///<summary>Command for adding a song to the playlist.</summary>
        ///<returns>A task, representing the action of adding a song.</returns>
        [Command("add")]
        public async Task AddMusic(string url)
        {
            try
            {
                var song = await audioManager.LoadSongAsync(url);
                audioManager.AudioPlayer.Enqueue(song);
                await ReplyAsync("Song was added to the queue.");
            }
            catch (System.Exception)
            {
                await ReplyAsync("Song could not be added!");
            }
        }

        ///<summary>Command for starting the audioplayer.</summary>
        ///<returns>A task, representing the action of starting the audioplayer.</returns>
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayMusic()
        {
            await audioManager.AudioPlayer.Start();
        }

        ///<summary>Command for skipping the song.</summary>
        ///<returns>A task, representing the action of skipping the song.</returns>
        [Command("next")]
        public async Task NextSong()
        {
            await audioManager.AudioPlayer.Next();
        }

        ///<summary>Command for removing all songs from the playlist.</summary>
        ///<returns>A task, representing the action of emptying the playlist.</returns>
        [Command("clear")]
        public async Task ClearPlaylist()
        {
            audioManager.AudioPlayer.ClearPlaylist();
            await ReplyAsync("All songs were removed from the playlist.");
        }

        ///<summary>Stops the player from playing music.</summary>
        ///<returns>A task, representing the action of stopping the player.</returns>
        [Command("stop")]
        public async Task StopPlayer()
        {
            await audioManager.AudioPlayer.Stop();
        }

        [Command("info")]
        public async Task GetInfo()
        {
            var currentSong = audioManager.AudioPlayer.currentSong;

            var field = new EmbedFieldBuilder()
                                .WithName("Remaining songs in playlist:")
                                .WithValue(audioManager.AudioPlayer.PlaylistCount);

            var embed = new EmbedBuilder()
                                .WithTitle(currentSong.Title)
                                .AddField(field)
                                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}