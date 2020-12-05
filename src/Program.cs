using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Discord.Audio;
using System.Diagnostics;
using VideoLibrary;
using tibrudna.djcort.src.Audio;
using tibrudna.djcort.src.Commands;

namespace tibrudna.djcort.src
{
    class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandService commandService;
        private CommandHandler commandHandler;
        private IServiceProvider provider;
        private AudioManager audioManager;

        public async Task MainAsync()
        {
            Console.CancelKeyPress += beforeExit;
            AppDomain.CurrentDomain.ProcessExit += beforeExit;

            audioManager = new AudioManager();

            provider = BuildServiceProvider();

            client = provider.GetService<DiscordSocketClient>();
            client.Log += Log;

            commandHandler = provider.GetService<CommandHandler>();
            await commandHandler.InstallCommandAsync();

            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN"));
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private async void beforeExit(object sender, EventArgs args)
        {
            await client.StopAsync();
            await client.LogoutAsync();
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<AudioManager>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
    }
}
