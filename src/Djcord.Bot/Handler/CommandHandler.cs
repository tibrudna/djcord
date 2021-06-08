using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Djcord.Bot.Command;

namespace Djcord.Bot.Handler
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModuleAsync<AudioCommand>(_services);
        }

        private async Task HandleCommandAsync(SocketMessage rawMessage)
        {
            var message = rawMessage as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;
            if (!message.HasCharPrefix('!', ref argPos)) return;
            if (message.Author.IsBot) return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}