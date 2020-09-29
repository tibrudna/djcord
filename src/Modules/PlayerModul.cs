using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
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
        public async Task JoinChannel(IVoiceChannel channel = null) => await playerService.JoinChannel(Context);

        [Command("play", RunMode = RunMode.Async)]
        public async Task StartPlaying()
        {
            await playerService.StartPlaying();
        }

        [Command("add")]
        public async Task AddToPlaylist(string url) => await playerService.AddToPlaylist(Context, url);
    }
}