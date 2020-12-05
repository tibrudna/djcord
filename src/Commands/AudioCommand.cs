using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using tibrudna.djcort.src.Audio;

namespace tibrudna.djcort.src.Commands
{
    public class AudioCommand : ModuleBase<SocketCommandContext>
    {
        private readonly AudioManager audioManager;

        public AudioCommand(AudioManager audioManager)
        {
            this.audioManager = audioManager;
        }

        [Command("echo")]
        public Task SayAsync([Remainder] string echo) => ReplyAsync(echo);

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinVoiceChannel()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) {
                await ReplyAsync("User is not in a voice channel!");
            }

            await audioManager.CreateNewAudioPlayer(channel);
        }

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

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayMusic()
        {
            await audioManager.AudioPlayer.Start();
        }

        [Command("next")]
        public async Task NextSong()
        {
            await audioManager.AudioPlayer.Next();
        }
    }
}