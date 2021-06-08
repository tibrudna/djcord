using System.Threading.Tasks;
using Discord.Commands;

namespace Djcord.Bot.Audio
{
    public interface IAudioService
    {
        Task PlayAsync(SocketCommandContext context);
        void Add(string url);
        void Next();
    }
}