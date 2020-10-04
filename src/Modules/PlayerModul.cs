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

        [Command("add")]
        public async Task AddToPlaylist(string url)
        {
            playerService.AddToPlaylist(url);
            await Context.Channel.SendMessageAsync("Song was added");
        }

        [Command("next")]
        public async Task NextSong()
        {
            playerService.NextSong();
            await Task.CompletedTask;
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task StartPlaying()
        {
            await playerService.StartPlaying();
        }

        [Command("now")]
        public async Task NowPlaying()
        {
            await Context.Channel.SendMessageAsync(embed: playerService.NowPlaying());
        }
    }
}