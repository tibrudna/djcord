using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using tibrudna.djcort.src.Handlers;
using tibrudna.djcort.src.Modules;
using tibrudna.djcort.src.Services;

namespace tibrudna.djcort.src
{
    class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandHandler handler;
        private CommandService commands;
        private IServiceProvider provider;

        public async Task MainAsync()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(beforeExit);
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => this.beforeExit(sender, args);

            provider = BuildServiceProvider();
            client = provider.GetService<DiscordSocketClient>();
            commands = provider.GetService<CommandService>();
            handler =  provider.GetService<CommandHandler>();

            await handler.InstallCommandAsync();

            client.Log += Log;

            await client.LoginAsync(TokenType.Bot, "TOKEN");
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
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<PlayerService>()
                .BuildServiceProvider();
    }
}
