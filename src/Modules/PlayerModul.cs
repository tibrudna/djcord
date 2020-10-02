using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using tibrudna.djcort.src.Exceptions;
using tibrudna.djcort.src.Services;

namespace tibrudna.djcort.src.Modules
{
    public class PlayerModule : ModuleBase<SocketCommandContext>
    {
        private PlayerService playerService;

        public PlayerModule(PlayerService playerService)
        {
            this.playerService = playerService;
        }


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

        [Command("play")]
        public async Task StartPlaying()
        {
            playerService.StartPlaying();
        }

        [Command("add")]
        public async Task AddToPlaylist(string url) => await playerService.AddToPlaylist(Context, url);

        [Command("next")]
        public async Task NextSong() => await playerService.NextSong();
    }
}