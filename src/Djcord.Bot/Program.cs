
using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Djcord.Bot.Audio;
using Djcord.Bot.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Djcord.Bot
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var _client = new DiscordSocketClient();
            _client.Log += Log;

            var token = Environment.GetEnvironmentVariable("TOKEN");

            var _commands = new CommandService();
            var _services = ConfigureService();
            var _handler = new CommandHandler(_client, _commands, _services);
            await _handler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureService() => new ServiceCollection()
                    .AddSingleton<IAudioService, AudioService>()
                    .BuildServiceProvider();

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
