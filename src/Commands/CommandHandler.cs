using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using tibrudna.djcort.src.Commands;

namespace tibrudna.djcort.src.Commands
{
    class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commandService;
        private readonly IServiceProvider services;
        private readonly string prefix;

        public CommandHandler(DiscordSocketClient client, CommandService commandService, IServiceProvider services)
        {
            this.client = client;
            this.commandService = commandService;
            this.services = services;
            this.prefix = Environment.GetEnvironmentVariable("PREFIX");

        }

        public async Task InstallCommandAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commandService.AddModuleAsync<AudioCommand>(services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;
            if (!message.HasStringPrefix(prefix, ref argPos) || message.Author.IsBot) return;

            var context = new SocketCommandContext(client, message);
            
            var result = await commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: services
            );
        }
    }
}