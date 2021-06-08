using System.Threading.Tasks;
using Discord.Commands;
using Djcord.Bot.Audio;

namespace Djcord.Bot.Command
{
    public class AudioCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IAudioService _audioService;

        public AudioCommand(IAudioService audioService)
        {
            _audioService = audioService;
        }

        [Command("echo")]
        public Task SayAsync([Remainder] string text) => ReplyAsync(text);

        [Command("add")]
        public async Task AddSong(string url)
        {
            _audioService.Add(url);
            await Context.Channel.SendMessageAsync("Song was added to the queue");
        }

        [Command("next")]
        public async Task NextSong()
        {
            _audioService.Next();
            await Task.CompletedTask;
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task Play()
        {
            await _audioService.PlayAsync(Context);
        }
    }
}