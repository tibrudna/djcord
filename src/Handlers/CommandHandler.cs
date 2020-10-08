using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using tibrudna.djcort.src.Modules;

namespace tibrudna.djcort.src.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider provider;
        private readonly string prefix;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider provider)
        {
            this._client = client;
            this._commands = commands;
            this.provider = provider;
            prefix = Environment.GetEnvironmentVariable("PREFIX");
        }

        public async Task InstallCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModuleAsync<PlayerModule>(provider);
            await _commands.AddModuleAsync<PlaylistModule>(provider);
        }

        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!message.HasStringPrefix(prefix, ref argPos)) return;

            var context = new SocketCommandContext(_client, message);

            var result = await _commands.ExecuteAsync(context, argPos, provider);
        }
    }
}