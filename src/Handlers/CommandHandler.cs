using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using tibrudna.djcort.src.Modules;

namespace tibrudna.djcort.src.Handlers
{
    /// <summary>Handles incoming Messages.</summary>
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider provider;
        private readonly string prefix;

        /// <summary>Initialize a new instance of the CommandHandler class.</summary>
        /// <param name="client">The client, who connected to discord.</param>
        /// <param name="commands">The CommandService, which holds the services for the commands.</param>
        /// <param name="provider">The provider for further services.</param>
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider provider)
        {
            this._client = client;
            this._commands = commands;
            this.provider = provider;
            prefix = Environment.GetEnvironmentVariable("PREFIX");
        }

        /// <summary>Makes all command services available.</summary>
        /// <returns>This method as a task.</returns>
        public async Task InstallCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModuleAsync<PlayerModule>(provider);
            await _commands.AddModuleAsync<PlaylistModule>(provider);
            await _commands.AddModuleAsync<SongModule>(provider);
        }

        /// <summary>Proceesses incoming Messages.</summary>
        /// <returns>This method as a task.</returns>
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