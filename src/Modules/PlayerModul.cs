using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using tibrudna.djcort.src.Exceptions;
using tibrudna.djcort.src.Services;

namespace tibrudna.djcort.src.Modules
{
    /// <summary>Commands to controll the music player.</summary>
    public class PlayerModule : ModuleBase<SocketCommandContext>
    {
        private PlayerService playerService;

        /// <summary>Creates a new instance of the PlayerModule class.</summary>
        /// <param name="playerService">The service which controlls the music player.</param>
        public PlayerModule(PlayerService playerService)
        {
            this.playerService = playerService;
        }

        /// <summary>Lets the music player join the channel in which the user is currently.</summary>
        /// <param name="channel">The channel, the music player should conntect to.</param>
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            try
            {
                await playerService.JoinChannel(Context.User);
            }
            catch (UserNotInVoiceChannelException exception)
            {

                await Context.Channel.SendMessageAsync(exception.Message);
            }
        }

        /// <summary>Skip the currently playing song.</summary>
        [Command("next")]
        public async Task NextSong()
        {
            playerService.NextSong();
            await Task.CompletedTask;
        }

        /// <summary>Starts the music player.</summary>
        [Command("start")]
        public Task StartPlayer()
        {
            playerService.Start();
            return Task.CompletedTask;
        }

        /// <summary>Returns information about the currently playing song.</summary>
        [Command("now")]
        public async Task NowPlaying()
        {
            await Context.Channel.SendMessageAsync(embed: playerService.NowPlaying());
        }
    }
}